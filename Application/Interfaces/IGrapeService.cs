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
    }
}
