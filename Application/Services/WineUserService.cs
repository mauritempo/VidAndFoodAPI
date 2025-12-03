using Application.Interfaces;
using Application.mapper;
using Application.Models.Response.User;
using Application.Models.Response.Wines;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Domain.Model.Shared;

namespace Application.Services
{
    public class WineUserService : IWineUserService
    {
        private readonly IWineUserRepository _wineUserRepository;
        private readonly IWineFavouriteRepository _wineFavouriteRepository;
        private readonly ICurrentUser _currentUser;
        public WineUserService(IWineUserRepository wineUserRepository, IWineFavouriteRepository wineFavouriteRepository, ICurrentUser currentUser)
        {
            _wineUserRepository = wineUserRepository;
            _wineFavouriteRepository = wineFavouriteRepository;
            _currentUser = currentUser;
        }



        public async Task<List<WineListItemDto>> GetHistoryList()
        {
            var userId = _currentUser.UserId;

            // 1. Llamamos al repo sin parámetros de página
            var historyItems = await _wineUserRepository.GetAllHistoryAsync(userId);

            // 2. Mapeamos directamente a la lista de DTOs
            var dtos = historyItems
                .Select(h => h.Wine.ToListItemDto())
                .ToList();

            return dtos;
        }


        public async Task RegisterConsumption(Guid wineId, string notes)
        {
            var userId = _currentUser.UserId;
            var history = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);

            if (history != null)
            {
                history.TimesConsumed++; // Incrementamos contador
                history.LastConsumedAt = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(notes))
                {
                    history.TastingNotes = notes;
                }

                await _wineUserRepository.UpdateAsync(history);
            }
            else
            {
                if (_currentUser.Role == Role.User)
                {
                    var count = await _wineUserRepository.GetCountByUserAsync(userId);

                    if (count >= 30)
                    {
                        throw new InvalidOperationException("Has alcanzado el límite de 30 vinos para cuentas gratuitas. Actualiza tu suscripción a Sommelier para guardar ilimitados.");
                    }
                }
                var newHistory = new WineUser
                {
                    UserId = userId,
                    WineId = wineId,
                    TimesConsumed = 1,
                    LastConsumedAt = DateTime.UtcNow,
                    TastingNotes = notes,
                };
                await _wineUserRepository.AddAsync(newHistory);
            }
        }



        public async Task<List<WineListItemDto>> ListFavoriteWines()
        {
            if (_currentUser.Role == Role.User)
            {
                throw new UnauthorizedAccessException("Esta funcionalidad es exclusiva para Sommeliers.");
            }

            var userId = _currentUser.UserId;
            var favorites = await _wineFavouriteRepository.GetFavorites(userId);
            var dtos = favorites.Select(f => f.Wine.ToListItemDto()).ToList();
            return dtos;
        }

        public async Task ToggleFavorite(Guid wineId)
        {
            if (_currentUser.Role == Role.User)
            {
                throw new UnauthorizedAccessException("Esta funcionalidad es exclusiva para Sommeliers.");
            }
            var userId = _currentUser.UserId;
            var existingFav = await _wineFavouriteRepository.GetFavouritesByUser(userId, wineId);
            if (existingFav != null)
            {
                await _wineFavouriteRepository.DeleteAsync(existingFav);
            }
            else
            {
                var newFav = new WineFavorite
                {
                    UserId = userId,
                    WineId = wineId,
                    CreatedAt = DateTime.UtcNow
                };
                await _wineFavouriteRepository.AddAsync(newFav);
            }
        }

        public async Task<UserWineStatusDto> GetUserWineStatus(Guid wineId)
        {
            var userId = _currentUser.UserId;
            var history = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);

            bool isFavorite = await _wineFavouriteRepository.IsFavoriteAsync(userId, wineId);

            return new UserWineStatusDto
            {
                WineId = wineId,
                IsInHistory = history != null,
                IsFavorite = isFavorite,

                // Datos del historial si existen
                TimesConsumed = history?.TimesConsumed ?? 0,
                PersonalNotes = history?.TastingNotes,
            };
        }
        public async Task RemoveFavorite(Guid wineId)
        {
            var userId = _currentUser.UserId;

            var existingFav = await _wineFavouriteRepository.GetFavouritesByUser(userId, wineId);

            if (existingFav != null)
            {
                await _wineFavouriteRepository.DeleteAsync(existingFav);
            }
        }


        public async Task RemoveFromHistory(Guid wineId)
        {
            var userId = _currentUser.UserId;

            var historyItem = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);

            if (historyItem != null)
            {
                await _wineUserRepository.DeleteAsync(historyItem);
            }
            else
            {
                throw new KeyNotFoundException("El vino no se encuentra en tu historial.");
            }
        }

    }
}
