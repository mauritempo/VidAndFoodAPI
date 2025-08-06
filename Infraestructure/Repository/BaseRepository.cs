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
        protected readonly WineDBContext _context;
        

        public BaseRepository(WineDBContext context)
        {
            _context = context;
            
        }

        public IEnumerable<T> Get()
        {
            return _context.Set<T>().ToList();
        }
    }
}
