using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IWineUserRepository : IBaseRepository<WineUser>
    {
        Task<WineUser?> GetHistoryItemAsync(Guid userId, Guid wineId);

        Task<int> CountHistoryAsync(Guid userId);

        Task<List<WineUser>> GetAllHistoryAsync(Guid userId);

        Task<int> GetCountByUserAsync(Guid userId);

        Task DeleteAsync(WineUser wineUser);

    }
}
