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
        // En Infrastructure/Repository/WineUserRepository.cs

        public async Task<(List<WineUser> Items, int TotalCount)> GetPagedHistoryAsync(Guid userId, int page, int pageSize)
        {
            var query = _context.Set<WineUser>()
                .AsNoTracking()
                .Where(h => h.UserId == userId)
                .Include(h => h.Wine)                // Traer el Vino
                    .ThenInclude(w => w.WineGrapeVarieties) // Traer relación intermedia
                        .ThenInclude(wg => wg.Grape)        // Traer nombre de la Uva
                                                            // Ordenamos por fecha de consumo (el más reciente arriba)
                                                            // Si LastConsumedAt es nulo, usa CreatedAt (seguridad)
                .OrderByDescending(h => h.LastConsumedAt ?? h.CreatedAt);

            // 1. Contar total de registros
            var totalCount = await query.CountAsync();

            // 2. Aplicar paginación
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }







    }
}