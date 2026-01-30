using Application.Interfaces;
using Application.Models.Request.Request;
using Application.Models.Response.Rating;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly IUserRepository _userRepository;


            public RatingService(IRatingRepository ratingRepository, IWineRepository wineRepository, ICurrentUser currentUser, IUserRepository userRepository)
            {
                _ratingRepository = ratingRepository;
                _wineRepository = wineRepository;
                _currentUser = currentUser;
                _userRepository = userRepository;
            }

            private static bool IsBySommelier(Role role) =>
                role == Role.Admin || role == Role.Sommelier;

            public async Task RateWineAsync(Guid wineUuId, RateWineRequest request)
            {
                var userUuId = _currentUser.UserId;
                var role = _currentUser.Role;

                if (userUuId == Guid.Empty)
                {
                    throw new UnauthorizedAccessException("No se pudo identificar al usuario desde el token.");
                }

                var userExists = await _userRepository.GetByIdAsync(userUuId);

                if (userExists is null)
                {
                    throw new KeyNotFoundException($"El usuario con ID {userUuId} no existe en la base de datos.");
                }

                if (request.Score < 1 || request.Score > 5)
                    throw new ArgumentException("El puntaje debe estar entre 1 y 5.");
                if (request.Score < 1 || request.Score > 5)
                    throw new ArgumentException("El puntaje debe estar entre 1 y 5.");


                bool isBySommelier; // 1. Declarar afuera

                if (role == Role.Admin || role == Role.Sommelier)
                {
                    isBySommelier = true;
                }
                else
                {
                    isBySommelier = false;
                }

                bool sommelier = IsBySommelier(role);

                var existing = await _ratingRepository.GetByUserAndWineAsync(userUuId, wineUuId);

                if (existing is not null)
                {
                    existing.Score = request.Score;
                    existing.Review = request.Review;
                    existing.IsSommelier = sommelier;
                    existing.UpdatedAt = DateTime.UtcNow;
                    await _ratingRepository.UpdateAsync(existing);
                }
                else
                {
                    var rating = new Rating
                    {
                        UserUuId = userUuId, // Aquí es donde fallaba si userUuId no era válido
                        WineUuId = wineUuId,
                        Score = request.Score,
                        Review = request.Review,
                        IsSommelier = sommelier,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _ratingRepository.AddAsync(rating);
                }
                await UpdateWineStatistics(wineUuId);
            }
                
        
            public async Task DeleteRateAsync(Guid wineUuId)
                {
                    var userUuId = _currentUser.UserId;
                    var role = _currentUser.Role;

                    //if (role == Role.Admin)
                    //{
                    //    rating = await _ratingRepository.GetByWineAsync(wineUuId);
                    //}

                    var existing = await _ratingRepository
                        .GetByUserAndWineAsync(userUuId, wineUuId);

                    

                    if (existing is not null)
                    {
                        existing.IsSommelier = false;
                        existing.UpdatedAt = DateTime.UtcNow;
                        existing.IsActive = false;

                        await _ratingRepository.UpdateAsync(existing);
                    }
                    else
                    {
                    throw new ArgumentException("No se pudo eliminar el Puntaje");
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


