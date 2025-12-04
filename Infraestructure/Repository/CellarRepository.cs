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
    public class CellarRepository : BaseRepository<CellarPhysics>, ICellarRepository
    {
        public CellarRepository(WineDBContext context) : base(context)
        {
        }

        public async Task<List<CellarPhysics>> GetUserCellarsAsync(Guid userId)
        {
            return await _context.Set<CellarPhysics>()
                .AsNoTracking()
                .Where(c => c.UserId == userId)

                .Include(c => c.Items)
                .ToListAsync();
        }
    }
}

