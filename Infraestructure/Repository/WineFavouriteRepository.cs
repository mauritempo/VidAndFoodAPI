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
    public class WineFavouriteRepository : BaseRepository<WineFavorite>, IWineFavouriteRepository
    {
        public WineFavouriteRepository(WineDBContext context) : base(context)
        {
        }
        public async Task<List<WineFavorite>> GetFavorites(Guid userId)
        {
            return await _context.Set<WineFavorite>()
                .AsNoTracking()
                .Where(h => h.UserId == userId)
                .Include(h => h.Wine)
                    .ThenInclude(w => w.Ratings)
                .Include(h => h.Wine)
                    .ThenInclude(w => w.WineGrapeVarieties)
                        .ThenInclude(wg => wg.Grape)
                .AsSplitQuery()
                .OrderByDescending(h =>  h.CreatedAt)
                .ToListAsync();
        }

        public async Task<WineFavorite?> GetFavouritesByUser(Guid userId, Guid wineId)
        {
            return await _context.Set<WineFavorite>()
                .FirstOrDefaultAsync(f => f.UserId == userId && f.WineId == wineId);
        }

        public async Task<bool> IsFavoriteAsync(Guid userId, Guid wineId)
        {
            return await _context.Set<WineFavorite>()
                .AnyAsync(f => f.UserId == userId && f.WineId == wineId);
        }

        public async Task DeleteAsync(WineFavorite favorite)
        {
            _context.Set<WineFavorite>().Remove(favorite);
            await _context.SaveChangesAsync();
        }
        

    }
}
