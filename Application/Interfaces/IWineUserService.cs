using Application.Models.Response.User;
using Application.Models.Response.Wines;
using Domain.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWineUserService
    {
        Task<List<WineListItemDto>> GetHistoryList();
        Task RegisterConsumption(Guid wineId);
        Task<List<WineListItemDto>> ListFavoriteWines();
        Task ToggleFavorite(Guid wineId);
        Task<UserWineStatusDto> GetUserWineStatus(Guid wineId);
        Task RemoveFavorite(Guid wineId);
        Task RemoveFromHistory(Guid wineId);
    }
}
