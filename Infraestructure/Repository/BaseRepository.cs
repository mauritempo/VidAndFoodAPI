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
        public readonly WineDBContext _context;
        

        public BaseRepository(WineDBContext context)
        {
            _context = context;
            
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetById<TId>(TId id)
        {
            return await _context.Set<T>().FindAsync(new object[] { id });
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
           
        }
        public void Update(T entity)
        {
           _context.Set<T>().Update(entity);
            
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
