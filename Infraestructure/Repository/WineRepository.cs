using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Model.Enums.Wines.Criteria;
using Infrastructure.Repository.common;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Infrastructure.Repository
{
    public class WineRepository : BaseRepository<Wine>, IWineRepository
    {
        public WineRepository(WineDBContext context) : base(context)
        {
        }
        public async Task<(List<Wine> Items, int TotalCount)> GetAdminListAsync(
            WineAdminCriteria criteria, 
            int page,
            int pageSize,
            string sortOrder)
        {
            var query = _context.Set<Wine>().AsNoTracking(); // Acceso seguro al DbSet

            if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
            {
                var term = criteria.SearchTerm.ToLower();
                query = query.Where(w => w.Name.ToLower().Contains(term) ||
                                         w.WineryName.ToLower().Contains(term));
            }

            if (criteria.ShowDeleted.HasValue)
            {
                bool isActiveState = !criteria.ShowDeleted.Value;
                query = query.Where(w => w.IsActive == isActiveState);
            }
            else
            {
                query = query.Where(w => w.IsActive);
            }

            // --- CONTEO TOTAL ---
            var totalCount = await query.CountAsync();

            var sort = sortOrder?.ToLower() ?? "name_asc"; // Valor por defecto seguro

            query = sort switch
            {
                "year_desc" => query.OrderByDescending(w => w.VintageYear),
                "year_asc" => query.OrderBy(w => w.VintageYear),

                "name_desc" => query.OrderByDescending(w => w.Name),
                "name_asc" => query.OrderBy(w => w.Name),

                _ => query.OrderBy(w => w.Name)
            };

            // --- PAGINACIÓN ---
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // 2. BUSCADOR PÚBLICO (Usa WineSearchCriteria)
        public async Task<(List<Wine> Items, int TotalCount)> SearchAsync(WineSearchCriteria criteria,int page,int pageSize,string sortOrder)
        {
            var query = _context.Set<Wine>()
                .AsNoTracking()
                .Where(w => w.IsActive);


            if (!string.IsNullOrWhiteSpace(criteria.Winery))
            {
                var term = criteria.Winery.ToLower();
                query = query.Where(w => w.Name.ToLower().Contains(term) ||
                                         w.WineryName.ToLower().Contains(term));
            }

            if (criteria.GrapeId.HasValue)
            {
                query = query.Where(w => w.WineGrapeVarieties.Any(gv => gv.GrapeId == criteria.GrapeId));
            }

            if (criteria.MinPrice.HasValue)
                query = query.Where(w => w.Price >= criteria.MinPrice.Value);

            if (criteria.MaxPrice.HasValue)
                query = query.Where(w => w.Price <= criteria.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(criteria.Region))
                query = query.Where(w => w.RegionName == criteria.Region);

            var totalCount = await query.CountAsync();

            var sort = sortOrder?.ToLower() ?? "rating_desc";

            query = sort switch
            {

                "rating_desc" => query.OrderByDescending(w => w.AverageScore),

                "year_desc" => query.OrderByDescending(w => w.VintageYear),
                "year_asc" => query.OrderBy(w => w.VintageYear),

                "name_asc" or "name" => query.OrderBy(w => w.Name),

                
            };
            query = query
                .Include(w => w.WineGrapeVarieties)
                .ThenInclude(gv => gv.Grape);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Wine?> GetWithDetailsAsync(Guid uuid)
        {
            return await _context.Set<Wine>()
                .AsNoTracking()
                .Include(w => w.WineGrapeVarieties)
                    .ThenInclude(gv => gv.Grape)
                .FirstOrDefaultAsync(w => w.UuId == uuid);
        }
        public async Task<Wine?> GetForUpdateAsync(Guid uuid)
        {
            return await _context.Set<Wine>()
                .Include(w => w.WineGrapeVarieties)
                .FirstOrDefaultAsync(w => w.UuId == uuid);
        }

        
    }
}