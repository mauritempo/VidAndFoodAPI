using Application.Models.Request.Wines;
using Application.Models.Response.Wines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWineService
    {
        Task<WineDetailDto> CreateWine(CreateWineRequest request);
        Task<WineDetailDto> UpdateWine(Guid id, UpdateWineRequest request);

        Task SoftDeleteWine(Guid id);

    }
}
