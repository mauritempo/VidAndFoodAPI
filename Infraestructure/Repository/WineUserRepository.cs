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
        public async Task<List<WineUser>> GetUserHistoryAsync(Guid userId)
        {
            return await _context.Set<WineUser>()
               .AsNoTracking()
               .Where(h => h.UserId == userId)
               .Include(h => h.Wine)
               .ToListAsync();
        }




        


    }
}