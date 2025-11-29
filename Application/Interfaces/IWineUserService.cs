using Application.Models.Response.User;
using Application.Models.Response.Wines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWineUserService
    {
        Task<List<WineListItemDto>> GetHistoryList(Guid userId);
        Task RegisterConsumption(Guid userId, Guid wineId, string notes);
        Task<UserWineStatusDto> GetUserWineStatus(Guid userId, Guid wineId);
    }
}
