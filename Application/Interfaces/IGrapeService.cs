using Application.Models.Request.Grape;
using Application.Models.Response;
using Domain.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGrapeService
    {
        Task<List<GrapeResponseDto>> GetAllGrapes();
        Task UpdateGrape(Guid id, UpdateGrapeRequest request);
        Task CreateGrape(NewGrape newGrape);
        Task DeleteGrape(Guid id);

    }
}
