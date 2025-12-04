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
    public class CellarItemRepository : BaseRepository<WineUserCellarItem>, ICellarItemRepository
    {
        public CellarItemRepository(WineDBContext context) : base(context)
        {
        }
        public async Task<(List<WineUserCellarItem> Items, int TotalCount)> GetInventoryAsync(Guid cellarId, int page, int pageSize)
        {
            var query = _context.Set<WineUserCellarItem>()
                .AsNoTracking()
                .Where(i => i.CellarPhysicsId == cellarId) // Nota: Usamos la FK directa
                .Include(i => i.Wine)
                    .ThenInclude(w => w.WineGrapeVarieties)
                        .ThenInclude(wg => wg.Grape)
                .OrderByDescending(i => i.DateAdded); // Asumo que se llama DateAdded o CreatedAt

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // 2. BUSCAR UN ITEM ESPECÍFICO (Movido aquí)
        public async Task<WineUserCellarItem?> GetItemAsync(Guid cellarId, Guid wineId)
        {
            return await _context.Set<WineUserCellarItem>()
                // Sin AsNoTracking porque seguro lo vas a querer editar (Update)
                .FirstOrDefaultAsync(i => i.CellarPhysicsId == cellarId && i.WineId == wineId);
        }

        // 3. BUSCADOR GLOBAL (Movido aquí)
        public async Task<List<WineUserCellarItem>> FindWineInUserCellarsAsync(Guid userId, Guid wineId)
        {
            return await _context.Set<WineUserCellarItem>()
               .AsNoTracking()
               .Include(i => i.CellarPhysics)
               .Where(i => i.CellarPhysics.UserId == userId && i.WineId == wineId)
               .ToListAsync();
        }
    }
}

