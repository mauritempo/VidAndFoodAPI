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
        Task<PagedResult<WineListItemDto>> GetHistoryList(int page, int pageSize);
        Task RegisterConsumption(Guid wineId, string notes);
        Task<PagedResult<WineListItemDto>> ListFavoriteWines(int page, int pageSize);
        Task ToggleFavorite(Guid wineId);
        Task<UserWineStatusDto> GetUserWineStatus(Guid wineId);

    }
}
