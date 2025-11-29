using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ICellearRepository : IBaseRepository<CellarPhysics>
    {
        Task<List<CellarPhysics>> GetUserCellarsAsync(Guid userId);
    }
}
