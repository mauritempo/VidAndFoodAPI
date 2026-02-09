using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IGrapeRepository : IBaseRepository<Grape>
    {
        Task<Grape?> GetWithDetailsAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);

        Task<int> CountAsync(Expression<Func<Grape, bool>> predicate);
    }
}
