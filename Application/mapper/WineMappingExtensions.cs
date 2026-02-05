using Application.Models.Request.Wines;
using Application.Models.Response;
using Application.Models.Response.Rating;
using Application.Models.Response.Wines;
using Domain.Entities;
using Domain.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.mapper
{
    public static class WineMappingExtensions
    {
        // 1. De REQUEST a Entidad
        public static Wine ToEntity(this CreateWineRequest request)
        {
            return new Wine
            {
                UuId = Guid.NewGuid(),
                Name = request.Name,
                WineryName = request.WineryName,
                RegionName = request.RegionName,
                VintageYear = request.VintageYear,
                Price = request.Price,
                LabelImageUrl = request.ImageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                WineGrapeVarieties = new List<WineGrapeVariety>()
            };
        }

        // 2. De Entidad a RESPONSE (Detalle)
        public static WineDetailDto ToDetailDto(this Wine entity)
        {
            if (entity == null) return null;
            return new WineDetailDto
            {
                Id = entity.UuId,
                Name = entity.Name,
                WineryName = entity.WineryName,
                RegionName = entity.RegionName,
                VintageYear = entity.VintageYear,
                Price = entity.Price,
                NotesTaste = entity.TastingNotes ?? "",
                Aroma = entity.Aroma ?? "",
                ImageUrl = entity.LabelImageUrl,
                AverageScore = entity.AverageScore,
                IsActive = entity.IsActive,
                IsWineDiscontinued = !entity.IsActive,
                // Mapeo de la lista de uvas
                Grapes = entity.WineGrapeVarieties?.Select(g => new GrapeResponseDto
                {
                    Id = g.GrapeId,
                    Name = g.Grape?.Name ?? "Sin nombre"
                }).ToList() ?? new List<GrapeResponseDto>(),

            };

        }

        public static WineAdminListItemDto ToAdminDto(this Wine entity)
        {
            return new WineAdminListItemDto
            {
                Id = entity.UuId,
                Name = entity.Name,
                WineryName = entity.WineryName,
                RegionName = entity.RegionName,
                Price = entity.Price,
                VintageYear = entity.VintageYear,
                ImageUrl = entity.LabelImageUrl,
                AverageScore = entity.AverageScore,
                // Concatenamos las uvas para mostrar algo rápido en la tabla
                GrapeNames = string.Join(", ", entity.WineGrapeVarieties.Select(wg => wg.Grape.Name)),
                IsWineDiscontinued = !entity.IsActive,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,

                Reviews = entity.Ratings?
                .Select(r => new WineReviewDto
                {
                    UserName = r.User.FullName,
                    Score = r.Score,
                    Review = r.Review,
                    CreatedAt = r.UpdatedAt ?? r.CreatedAt,
                    IsSommelierReview = r.IsSommelier // Usamos el campo de la entidad Rating
                }).OrderByDescending(r => r.IsSommelierReview) // Opcional: Sommeliers arriba
                  .ThenByDescending(r => r.CreatedAt)
                .ToList() ?? new List<WineReviewDto>()
            };
        }

        // 4. De Entidad a RESPONSE (Lista Pública)
        public static WineListItemDto ToListItemDto(this Wine entity)
        {
            if (entity == null) return null;

            return new WineListItemDto
            {
                Id = entity.UuId,
                Name = entity.Name,
                WineryName = entity.WineryName,
                RegionName = entity.RegionName,
                Price = entity.Price,
                VintageYear = entity.VintageYear,
                ImageUrl = entity.LabelImageUrl,
                AverageScore = entity.AverageScore,
                Aroma = entity.Aroma,
                NotesTaste = entity.TastingNotes,

                GrapeNames = string.Join(", ", entity.WineGrapeVarieties?
                    .Where(wg => wg?.Grape != null)
                    .Select(wg => wg.Grape.Name) ?? Array.Empty<string>()),

                IsWineDiscontinued = !entity.IsActive,

                Reviews = entity.Ratings?
                    .Where(r => r != null) 
                    .Select(r => new WineReviewDto
                    {
                        Id = r.UuId,
                        UserName = r.User?.FullName ?? "Usuario Anónimo",
                        Score = r.Score,
                        Review = r.Review ?? string.Empty,
                        CreatedAt = r.UpdatedAt ?? r.CreatedAt,
                        IsSommelierReview = r.IsSommelier
                    })
                    .OrderByDescending(r => r.IsSommelierReview)
                    .ThenByDescending(r => r.CreatedAt)
                    .ToList() ?? new List<WineReviewDto>()
            };
        }
    }
}
