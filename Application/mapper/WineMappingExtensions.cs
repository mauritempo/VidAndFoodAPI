using Application.Models.Request.Wines;
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
                ImageUrl = entity.LabelImageUrl,
                AverageScore = entity.AverageScore,
                IsActive = entity.IsActive,
                // Mapeo de la lista de uvas
                Grapes = entity.WineGrapeVarieties?.Select(g => new GrapeDto
                {
                    Id = g.GrapeId,
                    Name = g.Grape?.Name ?? "Sin nombre"
                }).ToList() ?? new List<GrapeDto>()
            };
        }

        // 3. De Entidad a RESPONSE (Lista Admin)
        public static WineAdminListItemDto ToAdminDto(this Wine entity)
        {
            return new WineAdminListItemDto
            {
                Id = entity.UuId,
                Name = entity.Name,
                WineryName = entity.WineryName,
                Price = entity.Price,
                VintageYear = entity.VintageYear,
                IsActive = entity.IsActive
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
                Price = entity.Price,
                VintageYear = entity.VintageYear,
                AverageScore = entity.AverageScore,
                ImageUrl = entity.LabelImageUrl,
                GrapeNames = entity.WineGrapeVarieties != null && entity.WineGrapeVarieties.Any()
                    ? string.Join(", ", entity.WineGrapeVarieties.Select(g => g.Grape?.Name))
                    : "Blend"
            };
        }
    }
}
