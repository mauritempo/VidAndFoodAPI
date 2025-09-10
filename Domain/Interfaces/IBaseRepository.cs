using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {

        Task<List<T>> GetAll();
        Task<T> GetById<TId>(TId id);
        void Add(T item);
        void Update(T item);


    }
}
