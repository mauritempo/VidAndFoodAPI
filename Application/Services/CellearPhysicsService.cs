using Application.Interfaces;
using Application.Models.Request.Celllear; // Tus DTOs de Request
using Application.Models.Response.Cellar;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class CellarPhysicsService
    {
        private readonly ICellearRepository _cellarRepository;
        private readonly ICellearItemRepository _itemRepository;
        private readonly IWineUserService _wineUserService;

        public CellarPhysicsService(
            ICellearRepository cellarRepository,
            ICellearItemRepository itemRepository,
            IWineUserService wineUserService)
        {
            _cellarRepository = cellarRepository;
            _itemRepository = itemRepository;
            _wineUserService = wineUserService;
        }

        public async Task ConsumeFromCellar(Guid userId, ConsumeItemRequest request)
        {
            var item = await _itemRepository.GetItemAsync(request.CellarId, request.WineId);

            if (item == null) throw new KeyNotFoundException("Vino no encontrado en esta cava");

            if (item.Quantity < request.Quantity)
                throw new InvalidOperationException($"Stock insuficiente. Tienes {item.Quantity}.");

            item.Quantity -= request.Quantity;

            if (item.Quantity <= 0)
            {
                // Si el stock llega a 0, borramos el registro.
                await _itemRepository.DeleteAsync(item);
            }
            else
            {
                // Si sobra stock, actualizamos.
                await _itemRepository.UpdateAsync(item);
            }

            if (request.RegisterHistory)
            {
                await _wineUserService.RegisterConsumption(userId, request.WineId, request.Notes ?? "");
            }
        }
        public async Task AddWineToCellar(Guid userId, AddWineToCellarRequest request)
        {
            // A. Verificar si ya existe en esa cava
            var existingItem = await _itemRepository.GetItemAsync(request.CellarId, request.WineId);

            if (existingItem != null)
            {
                // CASO 1: Ya existe -> Sumamos cantidad
                existingItem.Quantity += request.Quantity;
                existingItem.DateAdded = DateTime.UtcNow;

                // Persistir Update
                await _itemRepository.UpdateAsync(existingItem);
            }
            else
            {
                // CASO 2: No existe -> Creamos nuevo ítem
                var newItem = new WineUserCellarItem
                {
                    CellarPhysicsId = request.CellarId,
                    WineId = request.WineId,
                    Quantity = request.Quantity,
                    DateAdded = DateTime.UtcNow,
                    PurchasePrice = request.PurchasePrice
                };

                // Persistir Insert
                await _itemRepository.AddAsync(newItem);
            }
            var cellar = await _cellarRepository.GetByIdAsync(request.CellarId); // Necesitarías este método o usar el que ya tenés

            // 2. Calcular ocupación actual
            int currentCount = cellar.Items.Sum(i => i.Quantity);

            // 3. Validar si explota la capacidad (Solo si Capacity tiene valor)
            if (cellar.Capacity.HasValue && (currentCount + request.Quantity) > cellar.Capacity.Value)
            {
                throw new InvalidOperationException($"No hay espacio suficiente. Capacidad: {cellar.Capacity}, Ocupado: {currentCount}.");
            }
        }

        public async Task<CellarPhysics> CreateCellar(Guid userId, CreateCellarRequest request)
        {
            var newCellar = new CellarPhysics
            {
                UserId = userId,
                Name = request.Name,
                // Mapear otros campos si existen
            };

            // Usamos el repo de Cavas
            return await _cellarRepository.AddAsync(newCellar);
        }

        public async Task<List<CellarSummaryDto>> ListUserCellars(Guid userId)
        {
            // Usamos el método específico del repo de Cavas que incluye los Items
            var cellars = await _cellarRepository.GetUserCellarsAsync(userId);

            // Mapeo a DTO con resumen de cantidad
            return cellars.Select(c => new CellarSummaryDto
            {
                Id = c.UuId,
                Name = c.Name,
                // Sumamos la cantidad de botellas de cada item para el resumen
                TotalBottles = c.Items?.Sum(i => i.Quantity) ?? 0
            }).ToList();
        }
    }
}