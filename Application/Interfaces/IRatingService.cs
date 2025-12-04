using Application.Models.Request.Request;
using Application.Models.Response.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRatingService
    {
        Task RateWineAsync(RateWineRequest request);
        Task UpdateWineStatistics(Guid wineId);
        Task<List<WineReviewDto>> GetWineReviews(Guid wineId);

    }
}
