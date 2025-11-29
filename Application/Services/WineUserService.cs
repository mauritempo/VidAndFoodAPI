using Application.Interfaces;
using Application.mapper;
using Application.Models.Response.User;
using Application.Models.Response.Wines;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Model.Shared;

namespace Application.Services
{
    public class WineUserService : IWineUserService
    {
        private readonly IWineUserRepository _wineUserRepository;
        private readonly IWineFavouriteRepository _wineFavouriteRepository;
        public WineUserService(IWineUserRepository wineUserRepository, IWineFavouriteRepository wineFavouriteRepository)
        {
            _wineUserRepository = wineUserRepository;
            _wineFavouriteRepository = wineFavouriteRepository;
        }



        public async Task<List<WineListItemDto>> GetHistoryList(Guid userId)
        {
            // 1. El repositorio te da la lista de relaciones (WineUser)
            var historyList = await _wineUserRepository.GetUserHistoryAsync(userId);

            // 2. Transformás esa lista en DTOs de vinos
            // Nota: Accedemos a la propiedad .Wine de cada registro del historial
            var dtos = historyList
                .Select(h => h.Wine.ToListItemDto())
                .ToList();

            return dtos;
        }


        public async Task RegisterConsumption(Guid userId, Guid wineId, string notes)
        {

            var history = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);

            if (history != null)
            {
                history.TimesConsumed++;// Incrementamos contador [cite: 128]
                history.LastConsumedAt = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(notes))
                {
                    history.TastingNotes = notes;
                }

                // Llamamos al nuevo método de update
                await _wineUserRepository.UpdateAsync(history);
            }
            else
            {
                var newHistory = new WineUser
                {
                    UserId = userId,
                    WineId = wineId,
                    TimesConsumed = 1,
                    LastConsumedAt = DateTime.UtcNow,
                    TastingNotes = notes,
                };

                // Pasamos la entidad completa para guardar notas y fechas
                await _wineUserRepository.AddAsync(newHistory);
            }
        }
        public async Task<PagedResult<WineListItemDto>> ListFavoriteWines(Guid userId, int page, int pageSize)
        {
            // CORRECCIÓN: Usamos el repo de FAVORITOS, no el de usuario
            var (favorites, totalCount) = await _wineFavouriteRepository.GetPagedFavouriteAsync(userId, page, pageSize);

            // Mapeo: WineFavorite -> Wine -> DTO
            var dtos = favorites
                .Select(f => f.Wine.ToListItemDto())
                .ToList();

            return new PagedResult<WineListItemDto>(dtos, totalCount, page, pageSize);
        }

        public async Task ToggleFavorite(Guid userId, Guid wineId)
        {
            // CORRECCIÓN: Usamos el método GetByPairAsync del repo de favoritos
            var existingFav = await _wineFavouriteRepository.GetByPairAsync(userId, wineId);

            if (existingFav != null)
            {
                // Si existe, BORRAMOS (Des-likear)
                await _wineFavouriteRepository.DeleteAsync(existingFav);
            }
            else
            {
                // Si no existe, CREAMOS (Likear)
                var newFav = new WineFavorite
                {
                    UserId = userId,
                    WineId = wineId,
                    CreatedAt = DateTime.UtcNow
                };

                // Usamos AddAsync del BaseRepository
                await _wineFavouriteRepository.AddAsync(newFav);
            }
        }

        public async Task<UserWineStatusDto> GetUserWineStatus(Guid userId, Guid wineId)
        {
            // Consultamos los dos repositorios por separado
            var history = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);

            // CORRECCIÓN: Usamos IsFavoriteAsync para ser más eficientes (devuelve bool)
            // O usamos GetByPairAsync si necesitamos el objeto. 
            // Aquí basta con saber si existe.
            bool isFavorite = await _wineFavouriteRepository.IsFavoriteAsync(userId, wineId);

            return new UserWineStatusDto
            {
                WineId = wineId,
                IsInHistory = history != null,
                IsFavorite = isFavorite,

                // Datos del historial si existen
                TimesConsumed = history?.TimesConsumed ?? 0,
                PersonalNotes = history?.TastingNotes,
                // PersonalRating = history?.Rating (Si agregaste rating al historial)
            };
        }
    }
    }
