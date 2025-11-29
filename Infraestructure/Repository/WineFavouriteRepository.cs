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
        public async Task<(List<WineFavorite> Items, int TotalCount)> GetPagedFavouriteAsync(Guid userId, int page, int pageSize)
        {
            var query = _context.Set<WineFavorite>()
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .Include(f => f.Wine)                // Traer el Vino
                    .ThenInclude(w => w.WineGrapeVarieties) // Traer relación intermedia
                        .ThenInclude(wg => wg.Grape)        // Traer nombre de la Uva
                .OrderByDescending(f => f.CreatedAt); // Los más nuevos arriba

            // Contar antes de paginar
            var totalCount = await query.CountAsync();

            // Paginar
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        public async Task<WineFavorite?> GetByPairAsync(Guid userId, Guid wineId)
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
