using Domain.Entities;
using Domain.Model.Enums.Wines.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IWineRepository : IBaseRepository<Wine>
    {
        Task<(List<Wine> Items, int TotalCount)> GetAdminListAsync(WineAdminCriteria criteria,int page,int pageSize,string sortOrder);

        Task<Wine?> GetForUpdateAsync(Guid uuid);

        Task<(List<Wine> Items, int TotalCount)> SearchAsync(WineSearchCriteria criteria, int page, int pageSize, string sortOrder);

        Task<Wine?> GetWithDetailsAsync(Guid uuid);

        Task<List<Wine>> GetAllWithRatingsAsync();

        Task<List<string>> GetUniqueWineriesAsync();

        Task<List<Wine>> GetTopRatedAsync();

    }
}
