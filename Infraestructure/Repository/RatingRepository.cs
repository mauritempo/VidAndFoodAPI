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

    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        public RatingRepository(WineDBContext context) : base(context) 
        {
        }
        public async Task<Rating?> GetByUserAndWineAsync(Guid userId, Guid wineId)
        {
            return await _context.Set<Rating>()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.WineId == wineId);
        }

        // 2. Obtener Estadísticas Matemáticas (Promedio y Cantidad)
        // Retorna una tupla (Promedio, CantidadTotal)
        public async Task<(double Average, int Count)> GetWineStatsAsync(Guid wineId)
        {
            var query = _context.Set<Rating>().Where(r => r.WineId == wineId);

            var count = await query.CountAsync();

            // Si no hay votos, el promedio es 0. Si hay, calculamos el promedio.
            var average = count > 0
                ? await query.AverageAsync(r => r.Score)
                : 0.0;

            return (average, count);
        }

        // 3. Listar Reseñas para la ficha del vino (Paginado)
        public async Task<List<Rating>> GetReviewsByWineAsync(Guid wineId, int page, int pageSize)
        {
            return await _context.Set<Rating>()
                .AsNoTracking()
                .Where(r => r.WineId == wineId && !string.IsNullOrEmpty(r.Review)) // Solo traer los que tienen texto
                .Include(r => r.User) // Incluir usuario para mostrar el nombre
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

