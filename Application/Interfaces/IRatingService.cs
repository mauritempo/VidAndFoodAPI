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
        Task RateWineAsync(Guid wineUuId, RateWineRequest request);

        Task DeleteRateAsync(Guid wineUuId);
        Task UpdateWineStatistics(Guid wineId);
        Task<List<WineReviewDto>> GetWineReviews(Guid wineId);

    }
}
