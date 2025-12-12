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
        //Task ConsumeFromCellar(ConsumeItemRequest request);
        Task AddWineToCellar(AddWineToCellarRequest request);

        Task<CellarPhysics> CreateCellar(CreateCellarRequest request);

        Task<List<CellarSummaryDto>> ListUserCellars();

    }
}
