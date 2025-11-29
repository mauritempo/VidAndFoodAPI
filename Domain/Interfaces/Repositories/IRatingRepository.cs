using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IRatingRepository : IBaseRepository<Rating>
    {
        Task<Rating?> GetByUserAndWineAsync(Guid userId, Guid wineId);

        Task<(double Average, int Count)> GetWineStatsAsync(Guid wineId);

        Task<List<Rating>> GetReviewsByWineAsync(Guid wineId, int page, int pageSize);

    }
}
