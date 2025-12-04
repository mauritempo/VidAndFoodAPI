using Application.Interfaces;
using Application.Models.Request.Request;
using Application.Models.Response.Rating;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
        public class RatingService : IRatingService
    {
            private readonly IRatingRepository _ratingRepository;
            private readonly IWineRepository _wineRepository;
            private readonly ICurrentUser _currentUser;

            public RatingService(IRatingRepository ratingRepository, IWineRepository wineRepository, ICurrentUser currentUser)
                {
                    _ratingRepository = ratingRepository;
                    _wineRepository = wineRepository;
                    _currentUser = currentUser;
            }

            public async Task RateWineAsync(RateWineRequest request)
            {
                var userId = _currentUser.UserId;
                var userRole = _currentUser.Role;

                if (request.Score < 1 || request.Score > 5)
                    throw new ArgumentException("El puntaje debe estar entre 1 y 5.");

                bool isRatingPublic = (userRole == Role.Sommelier);

                var existingRating = await _ratingRepository.GetByUserAndWineAsync(userId, request.WineId);

                if (existingRating != null)
                {
                    existingRating.Score = request.Score;
                    existingRating.Review = request.Review;
                    existingRating.CreatedAt = DateTime.UtcNow;
                    existingRating.IsPublic = isRatingPublic;

                    await _ratingRepository.UpdateAsync(existingRating);
                }
                else
                {
                    var newRating = new Rating
                    {
                        UserId = userId,
                        WineId = request.WineId,
                        Score = request.Score,
                        Review = request.Review,
                        CreatedAt = DateTime.UtcNow,

                        IsPublic = isRatingPublic
                    };

                    await _ratingRepository.AddAsync(newRating);
                }
                if (isRatingPublic)
                {
                    await UpdateWineStatistics(request.WineId);
                }
            }

            private async Task UpdateWineStatistics(Guid wineId)
            {
                var (newAverage, newCount) = await _ratingRepository.GetWineStatsAsync(wineId);

                var wine = await _wineRepository.GetByIdAsync(wineId);

                if (wine != null)
                {
                    wine.AverageScore = newAverage;

                    await _wineRepository.UpdateAsync(wine);
                }
            }

            public async Task<List<WineReviewDto>> GetWineReviews(Guid wineId)
            {
                var reviews = await _ratingRepository.GetReviewsByWineAsync(wineId);


                return reviews.Select(r => new WineReviewDto
                {
                    Id = r.UuId,
                    UserName = r.User?.FullName ?? "Anónimo", // Asumiendo que User tiene Name
                    Score = r.Score,
                    Review = r.Review,
                    CreatedAt = r.CreatedAt
                }).ToList();
            }

            Task IRatingService.UpdateWineStatistics(Guid wineId)
            {
                return UpdateWineStatistics(wineId);
            }
    }
    }

