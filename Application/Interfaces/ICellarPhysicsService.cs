using Application.Models.Request.Celllear;
using Application.Models.Response.Cellar;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICellarPhysicsService
    {
        Task ConsumeFromCellar(Guid userId, ConsumeItemRequest request);
        Task AddWineToCellar(Guid userId, AddWineToCellarRequest request);
        Task<CellarPhysics> CreateCellar(Guid userId, CreateCellarRequest request);

        Task<List<CellarSummaryDto>> ListUserCellars(Guid userId);
    }
}
