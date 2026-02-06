using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Repository.common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{

    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        public RatingRepository(WineDBContext context) : base(context) 
        {
        }
        public async Task<Rating?> GetByUserAndWineAsync(Guid userId, Guid wineId)
        {
            return await _context.Set<Rating>()
                .FirstOrDefaultAsync(r => r.UserUuId == userId && r.WineUuId == wineId && r.IsActive == true);
        }

        public async Task<(double Average, int Count)> GetWineStatsAsync(Guid wineId)
        {
            var query = _context.Set<Rating>()
                .Where(r => r.WineUuId == wineId && r.IsActive == true);

            var count = await query.CountAsync();

            
            var average = count > 0
                ? await query.AverageAsync(r => (double)r.Score)
                : 0.0;

            return (average, count);
        }

        public async Task<List<Rating>> GetReviewsByWineAsync(Guid wineId) 
        {
            return await _context.Set<Rating>()
                .AsNoTracking()
                .Where(r => r.WineUuId == wineId && !string.IsNullOrEmpty(r.Review))
                .Where(r => r.IsActive == true)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}

