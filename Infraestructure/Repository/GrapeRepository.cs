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
    public class GrapeRepository : BaseRepository<Grape>, IGrapeRepository
    {
        public GrapeRepository(WineDBContext context) : base(context)
        {
        }

        public async Task<Grape?> GetWithDetailsAsync(Guid id)
        {
            return await _context.Set<Grape>()
                .Include(g => g.WineGrapeVarieties)
                .FirstOrDefaultAsync(g => g.UuId == id);
        }
        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            var nameLower = name.Trim().ToLower();

            var query = _context.Set<Grape>().AsQueryable();
            query = query.Where(g => g.Name.ToLower() == nameLower);

            if (excludeId.HasValue)
            {
                query = query.Where(g => g.UuId != excludeId.Value);
            }
            return await query.AnyAsync();
        }
    }
}