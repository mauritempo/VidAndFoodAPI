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

        public IEnumerable<T> Get()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
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
