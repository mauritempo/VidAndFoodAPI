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
                .FirstOrDefaultAsync(r => r.UserId == userId && r.WineId == wineId);
        }
        public async Task<(double Average, int Count)> GetWineStatsAsync(Guid wineId)
        {
            var query = _context.Set<Rating>().Where(r => r.WineId == wineId);

            var count = await query.CountAsync();

            
            var average = count > 0
                ? await query.AverageAsync(r => r.Score)
                : 0.0;

            return (average, count);
        }

        public async Task<List<Rating>> GetReviewsByWineAsync(Guid wineId, int page, int pageSize)
        {
            return await _context.Set<Rating>()
                .AsNoTracking()
                .Where(r => r.WineId == wineId && !string.IsNullOrEmpty(r.Review)) // Solo traer los que tienen texto
                .Where(r => r.IsPublic == true)
                .Include(r => r.User) // Incluir usuario para mostrar el nombre
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

