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
            var query = _context.Set<WineFavorite>()
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .Include(f => f.Wine)
                    .ThenInclude(w => w.WineGrapeVarieties)
                        .ThenInclude(wg => wg.Grape)
                .OrderByDescending(f => f.CreatedAt);

            // CORRECCIÓN: Debes ejecutar la query con ToListAsync()
            return await query.ToListAsync();
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
