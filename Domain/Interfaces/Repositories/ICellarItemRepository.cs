using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ICellarItemRepository : IBaseRepository<WineUserCellarItem>
    {
        Task<(List<WineUserCellarItem> Items, int TotalCount)> GetInventoryAsync(Guid cellarId, int page, int pageSize);
        Task<WineUserCellarItem?> GetItemAsync(Guid cellarId, Guid wineId);
        Task<List<WineUserCellarItem>> FindWineInUserCellarsAsync(Guid userId, Guid wineId);
    }
}
