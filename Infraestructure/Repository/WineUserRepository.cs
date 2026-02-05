using Application.mapper;
using Application.Models.Response.Wines;
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
    public class WineUserRepository : BaseRepository<WineUser>, IWineUserRepository
    {
        public WineUserRepository(WineDBContext context) : base(context)
        {
        }

         public async Task<WineUser?> GetHistoryItemAsync(Guid userId, Guid wineId)
         {
                   return await _context.Set<WineUser>()
                        // No usamos AsNoTracking porque queremos modificarlo y guardar cambios (Update)
                        .FirstOrDefaultAsync(h => h.UserId == userId && h.WineId == wineId);
         }

        public async Task<int> CountHistoryAsync(Guid userId)
        {
            return await _context.Set<WineUser>()
                .AsNoTracking()
                .CountAsync(h => h.UserId == userId);
        }



        public async Task<List<WineUser>> GetAllHistoryAsync(Guid userId)
        {
            return await _context.Set<WineUser>()
                .AsNoTracking()
                .Where(h => h.UserId == userId)
                .Include(h => h.Wine)
                    .ThenInclude(w => w.Ratings)
                .Include(h => h.Wine)
                    .ThenInclude(w => w.WineGrapeVarieties)
                        .ThenInclude(wg => wg.Grape)
                .AsSplitQuery()
                .OrderByDescending(h => h.LastConsumedAt ?? h.CreatedAt)
                .ToListAsync();
        }
        public async Task<int> GetCountByUserAsync(Guid userId)
        {
            return await _context.Set<WineUser>()
                .Where(x => x.UserId == userId)
                .CountAsync();
        }

        public async Task DeleteAsync(WineUser wineUser)
        {
            _context.Set<WineUser>().Remove(wineUser);
            await _context.SaveChangesAsync();
        }



    }
}