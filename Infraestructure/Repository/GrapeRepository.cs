using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Repository.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GrapeRepository : BaseRepository<Grape>, IGrapeRepository
    {
        public GrapeRepository(WineDBContext context) : base(context) 
        {
        }
    }
}
