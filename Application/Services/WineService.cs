using Application.Interfaces;
using Application.mapper;
using Application.Models.Request.Wines;
using Application.Models.Response.Wines;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Domain.Model.Enums.Wines.Criteria;
using Domain.Model.Enums.Wines.Criteria.enums;
using Domain.Model.Shared; // Para PagedResult

namespace Application.Services
{
    public class WineService  : IWineService 
    {
        private readonly IWineRepository _wineRepository;
        private readonly ICurrentUser _currentUser;

        public WineService(IWineRepository wineRepository, ICurrentUser currentUser)
        {
            _wineRepository = wineRepository;
            _currentUser = currentUser;
        }

        // ==========================================================
        //  ADMIN METHODS
        // ==========================================================

        public async Task<WineDetailDto> CreateWine(CreateWineRequest request)
        {
            var userRole = _currentUser.Role;
            if (userRole != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo los administradores pueden Crear");
            }
            var wine = request.ToEntity();

            if (request.Grapes != null && request.Grapes.Any())
            {
                foreach (var grapeId in request.Grapes)
                {
                    wine.WineGrapeVarieties.Add(new WineGrapeVariety
                    {
                        WineId = wine.UuId,
                        GrapeId = grapeId
                    });
                }
            }

            await _wineRepository.AddAsync(wine);

            return wine.ToDetailDto();
        }

        public async Task<WineDetailDto> UpdateWine(Guid id, UpdateWineRequest request)
        {
            var userRole = _currentUser.Role;
            if (userRole != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo los administradores pueden Actualizar.");
            }

            var wine = await _wineRepository.GetForUpdateAsync(id);

            if (wine == null)
                throw new KeyNotFoundException($"No se encontró el vino con ID {id}");

            wine.Name = request.Name;
            wine.Price = request.Price;
            wine.VintageYear = request.VintageYear;
            wine.WineryName = request.WineryName;
            wine.RegionName = request.RegionName;
            wine.LabelImageUrl = request.ImageUrl;

            UpdateGrapeVarieties(wine, request.Grapes);

            await _wineRepository.UpdateAsync(wine);

            return wine.ToDetailDto();
        }

        public async Task SoftDeleteWine(Guid id)
        {
            var userRole = _currentUser.Role;
            if (userRole != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo los administradores pueden Eliminar.");
            }

            var wine = await _wineRepository.GetForUpdateAsync(id);
            if (wine == null) throw new KeyNotFoundException($"Vino {id} no encontrado");

            wine.IsActive = false; // Soft Delete

            await _wineRepository.UpdateAsync(wine);
        }

        public async Task RestoreWine(Guid id)
        {
            var userRole = _currentUser.Role;
            if (userRole != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo los administradores pueden acceder a esta sección.");
            }
            var wine = await _wineRepository.GetForUpdateAsync(id);
            if (wine == null) throw new KeyNotFoundException($"Vino {id} no encontrado");

            wine.IsActive = true; // Restaurar

            await _wineRepository.UpdateAsync(wine);
        }

        public async Task<PagedResult<WineAdminListItemDto>> ListWinesAdmin(WineAdminFilterRequest filter,int page,int pageSize,WineAdminSort sort)
        {
            var userRole = _currentUser.Role;
            if (userRole != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo los administradores pueden acceder a esta sección.");
            }

            var criteria = new WineAdminCriteria
            {
                SearchTerm = filter.SearchTerm,
                ShowDeleted = filter.ShowDeleted,
            };

            string sortOrder = sort.ToString();
            var (items, totalCount) = await _wineRepository.GetAdminListAsync(criteria, page, pageSize, sortOrder);
            var dtos = items.Select(w => w.ToAdminDto()).ToList();

            return new PagedResult<WineAdminListItemDto>(dtos, totalCount, page, pageSize);
        }


        public async Task<WineDetailDto> GetWineById(Guid id)
        {
            var wine = await _wineRepository.GetWithDetailsAsync(id);

            if (wine == null || !wine.IsActive)
                throw new KeyNotFoundException("Vino no encontrado");

            // Mapeo Manual
            return wine.ToDetailDto();
        }

        public async Task<PagedResult<WineListItemDto>> SearchWines(WineFilterRequest request,int page,int pageSize,WineSort sort)
        {
            var criteria = new WineSearchCriteria
            {
                Winery = request.Term,
                GrapeId = request.GrapeId,
                Region = request.Region,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
            };

            string sortString = sort.ToString();

            var (wines, totalCount) = await _wineRepository.SearchAsync(criteria, page, pageSize, sortString);

            var dtos = wines.Select(w => w.ToListItemDto()).ToList();

            return new PagedResult<WineListItemDto>(dtos, totalCount, page, pageSize);
        }


        private void UpdateGrapeVarieties(Wine wine, List<Guid> newGrapeIds)
        {
            if (newGrapeIds == null) return;

            var toRemove = wine.WineGrapeVarieties
                .Where(existing => !newGrapeIds.Contains(existing.GrapeId))
                .ToList();

            foreach (var item in toRemove)
            {
                wine.WineGrapeVarieties.Remove(item);
            }

            var existingIds = wine.WineGrapeVarieties.Select(x => x.GrapeId).ToList();
            var toAdd = newGrapeIds
                .Where(id => !existingIds.Contains(id))
                .ToList();

            foreach (var grapeId in toAdd)
            {
                wine.WineGrapeVarieties.Add(new WineGrapeVariety
                {
                    WineId = wine.UuId,
                    GrapeId = grapeId
                });
            }
        }
    }
}