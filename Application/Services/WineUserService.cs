using Application.Interfaces;
using Application.mapper;
using Application.Models.Response.User;
using Application.Models.Response.Wines;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Model.DTOs.Wines.Domain.Model.DTOs.Wines;
using Domain.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WineUserService : IWineUserService
    {
        private readonly IWineUserRepository _wineUserRepository;
        private readonly IWineFavouriteRepository wineFavouriteRepository;
        public WineUserService(IWineUserRepository wineUserRepository)
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
        public async Task<UserWineStatusDto> GetUserWineStatus(Guid userId, Guid wineId)
        {
            // Consultamos las dos tablas en paralelo o secuencial
            var history = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);
            var favorite = await wineFavouriteRepository.GetByIdAsync(userId, wineId);

            return new UserWineStatusDto
            {
                WineId = wineId,
                // Lógica simple de booleanos
                IsInHistory = history != null,
                IsFavorite = favorite != null,

                // Mapeo condicional
                TimesConsumed = history?.TimesConsumed ?? 0,
                PersonalNotes = history?.TastingNotes,
                // PersonalRating = history?.Rating
            };
        }

        //public async Task<PagedResult<WineListItemDto>> ListFavoriteWines(Guid userId, int page, int pageSize)
        //{
        //    // 1. Llamar al repositorio
        //    var (favorites, totalCount) = await _wineUserRepository.GetFavoritesAsync(userId, page, pageSize);

        //    // 2. Mapeo Manual (Usando tus extensiones)
        //    // Nota: La entidad 'WineFavorite' tiene una propiedad de navegación 'Wine'
        //    var dtos = favorites
        //        .Select(f => f.Wine.ToListItemDto()) // Reusamos el método que creamos antes
        //        .ToList();

        //    return new PagedResult<WineListItemDto>(dtos, totalCount, page, pageSize);
        //}


        // leer 
        // En WineUserService.cs

        //public async Task ToggleFavorite(Guid userId, Guid wineId)
        //{
        //    // 1. Verificamos si ya existe usando el nuevo método booleano
        //    bool isFavorite = await _wineUserRepository.IsFavoriteAsync(userId, wineId);

        //    if (isFavorite)
        //    {
        //        // 2. Si existe, lo borramos pasando los IDs
        //        await _wineUserRepository.RemoveFavoriteAsync(userId, wineId);
        //    }
        //    else
        //    {
        //        // 3. Si NO existe, lo agregamos pasando los IDs
        //        await _wineUserRepository.AddFavoriteAsync(userId, wineId);
        //    }
        //}



        // 3. Lógica de CONSULTA (El "Merge")
        // Aquí resolvemos tu duda: ¿Cómo sé si está en los dos lados?
        public async Task<UserWineStatusDto> GetUserWineStatus(Guid userId, Guid wineId)
        {
            // Consultamos las dos tablas en paralelo o secuencial
            var history = await _wineUserRepository.GetHistoryItemAsync(userId, wineId);
            var favorite = await _wineUserRepository.GetFavoriteAsync(userId, wineId);

            return new UserWineStatusDto
            {
                WineId = wineId,
                // Lógica simple de booleanos
                IsInHistory = history != null,
                IsFavorite = favorite != null,

                // Mapeo condicional
                TimesConsumed = history?.TimesConsumed ?? 0,
                PersonalNotes = history?.TastingNotes,
                // PersonalRating = history?.Rating
            };
        }
    }
}
