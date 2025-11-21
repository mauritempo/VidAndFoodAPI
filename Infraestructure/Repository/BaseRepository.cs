using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly DbContext _context;
        

        public BaseRepository(DbContext context)
        {
            _context = context;
            
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public void Update(T entity)
        {
           _context.Set<T>().Update(entity);
            
        }
        public async Task<T?> GetByIdAsync<TId>(TId id)
        {
            var entity = await _context.Set<T>().FindAsync(new object?[] { id! });
            return entity;
        }

        //public void Delete(int id) {
        //    var entity = _context.Set<T>().Find(id);
        //    if (entity != null)
        //    {
        //        _context.Set<T>().Remove(entity);
        //        _context.SaveChanges();
        //    }
        //}
    }
}
