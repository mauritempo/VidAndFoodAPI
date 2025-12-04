using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {

        Task<List<T>> GetAll();
        Task<T> AddAsync(T item);
        void Update(T item);
        Task<T?> GetByIdAsync<TId>(TId id);
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);


    }
}
