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



        public async Task<PagedResult<WineListItemDto>> GetHistoryList( int page, int pageSize)
        {
            var userId = _currentUser.UserId;
            var (historyItems, totalCount) = await _wineUserRepository.GetPagedHistoryAsync(userId, page, pageSize);
            var dtos = historyItems
                .Select(h => h.Wine.ToListItemDto())
                .ToList();
            return new PagedResult<WineListItemDto>(dtos, totalCount, page, pageSize);
        }


        public async Task RegisterConsumption(Guid wineId, string notes)
        {
            if (_currentUser.Role == Role.User)
            {
                throw new UnauthorizedAccessException("Mejora tu cuenta para registrar tus catas.");

            }

            var userId = _currentUser.UserId;

            var history = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);

            if (history != null)
            {
                history.TimesConsumed++;// Incrementamos contador [cite: 128]
                history.LastConsumedAt = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(notes))
                {
                    history.TastingNotes = notes;
                }

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

                await _wineUserRepository.AddAsync(newHistory);
            }
        }
        public async Task<PagedResult<WineListItemDto>> ListFavoriteWines(int page, int pageSize)
        {
            if (_currentUser.Role == Role.User)
            {
                throw new UnauthorizedAccessException("Esta funcionalidad es exclusiva para Sommeliers.");
            }

            var userId = _currentUser.UserId;
            var (favorites, totalCount) = await _wineFavouriteRepository.GetPagedFavouriteAsync(userId, page, pageSize);
            var dtos = favorites.Select(f => f.Wine.ToListItemDto()).ToList();

            return new PagedResult<WineListItemDto>(dtos, totalCount, page, pageSize);
        }

        public async Task ToggleFavorite(Guid wineId)
        {
            var userId = _currentUser.UserId;
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
    }
    }
