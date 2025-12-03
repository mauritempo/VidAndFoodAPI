using Application.Models.Request.Celllear;
using Application.Models.Response.Cellar;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Application.Interfaces; 

namespace Application.Services
{
    public class CellarPhysicsService : ICellarPhysicsService
    {
        private readonly ICellarRepository _cellarRepository;
        private readonly ICellarItemRepository _itemRepository;
        private readonly IWineUserService _wineUserService;
        private readonly ICurrentUser _currentUser; 

        public CellarPhysicsService(
            ICellarRepository cellarRepository,
            ICellarItemRepository itemRepository,
            IWineUserService wineUserService,
            ICurrentUser currentUser)
        {
            _cellarRepository = cellarRepository;
            _itemRepository = itemRepository;
            _wineUserService = wineUserService;
            _currentUser = currentUser;
        }

        public async Task ConsumeFromCellar(ConsumeItemRequest request)
        {
            var userId = _currentUser.UserId;
            var userRole = _currentUser.Role;

            if (userRole != Role.Sommelier)
            {
                throw new UnauthorizedAccessException("Solo los Sommeliers pueden gestionar el stock.");
            }
            var item = await _itemRepository.GetItemAsync(request.CellarId, request.WineId);

            if (item == null) throw new KeyNotFoundException("Vino no encontrado en esta cava");

            if (item.Quantity < request.Quantity)
                throw new InvalidOperationException($"Stock insuficiente. Tienes {item.Quantity}.");

            item.Quantity -= request.Quantity;

            if (item.Quantity <= 0)
            {
                await _itemRepository.DeleteAsync(item);
            }
            else
            {
                await _itemRepository.UpdateAsync(item);
            }

            if (request.RegisterHistory)
            {
                await _wineUserService.RegisterConsumption(request.WineId);
            }
        }

        public async Task AddWineToCellar(AddWineToCellarRequest request)
        {
            var userId = _currentUser.UserId;
            var userRole = _currentUser.Role;

            if (userRole != Role.Sommelier)
            {
                throw new UnauthorizedAccessException("Solo los Sommeliers pueden agregar vinos.");
            }

            var (items, _) = await _itemRepository.GetInventoryAsync(request.CellarId, 1, 1000);

            var cellar = await _cellarRepository.GetByIdAsync(request.CellarId);
            if (cellar == null) throw new KeyNotFoundException("Cava no encontrada");

            int currentCount = items.Sum(i => i.Quantity);

            if (cellar.Capacity.HasValue && (currentCount + request.Quantity) > cellar.Capacity.Value)
            {
                throw new InvalidOperationException($"No hay espacio suficiente. Capacidad: {cellar.Capacity}, Ocupado: {currentCount}, Intentas agregar: {request.Quantity}.");
            }

            var existingItem = await _itemRepository.GetItemAsync(request.CellarId, request.WineId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.DateAdded = DateTime.UtcNow;
                await _itemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new WineUserCellarItem
                {
                    CellarPhysicsId = request.CellarId,
                    WineId = request.WineId,
                    Quantity = request.Quantity,
                    DateAdded = DateTime.UtcNow,
                    PurchasePrice = request.PurchasePrice
                };
                await _itemRepository.AddAsync(newItem);
            }
        }

        public async Task<CellarPhysics> CreateCellar(CreateCellarRequest request)
        {
            var userId = _currentUser.UserId;
            var userRole = _currentUser.Role;
            if (userRole != Role.Sommelier)
            {
                throw new UnauthorizedAccessException("Solo los Sommeliers pueden crear cavas.");
            }
            var newCellar = new CellarPhysics
            {
                UserId = userId,
                Name = request.Name,
                Capacity = request.Capacity 
            };

            return await _cellarRepository.AddAsync(newCellar);
        }

        public async Task<List<CellarSummaryDto>> ListUserCellars()
        {
            var userId = _currentUser.UserId;
            var userRole = _currentUser.Role;
            if (userRole != Role.Sommelier)
            {
                throw new UnauthorizedAccessException("Función exclusiva para Sommeliers.");
            }
            var cellars = await _cellarRepository.GetUserCellarsAsync(userId);
            return cellars.Select(c => new CellarSummaryDto
            {
                Id = c.UuId,
                Name = c.Name,
                TotalBottles = c.Items?.Sum(i => i.Quantity) ?? 0,
                Capacity = c.Capacity
            }).ToList();
        }
    }
}