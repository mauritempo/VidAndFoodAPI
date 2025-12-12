using Application.Interfaces;
using Application.Models.Request.Grape;
using Application.Models.Response;
using Application.Models.Response.Grape;
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
    public class GrapeService :IGrapeService
    {
        private readonly IGrapeRepository _grapeRepository;
        private readonly ICurrentUser _currentUser;

        public GrapeService (IGrapeRepository grapeRepository, ICurrentUser currentUser)
        {
            _grapeRepository = grapeRepository;
            _currentUser = currentUser;
        }
        public async Task<List<GrapeResponseDto>> GetAllGrapes()
        {
            var grapes = await _grapeRepository.GetAll();
            
            return grapes.Select(g => new GrapeResponseDto
            {
                Id = g.UuId,   
                Name = g.Name
            }).ToList();
        }
        public async Task<List<GrapeListForAdmin>> GetAllGrapesAdmin()
        {
            var grapes = await _grapeRepository.GetAll();

            return grapes.Select(g => new GrapeListForAdmin
            {
                Id = g.UuId,
                Name = g.Name,
                UpdatedAt = g.UpdatedAt,
                CreatedAt = g.CreatedAt,

            }).ToList();
        }



        public async Task CreateGrape(NewGrape newGrape)
        {
            if (_currentUser.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo administradores pueden editar uvas.");
            }

            bool exists = await _grapeRepository.ExistsByNameAsync(newGrape.Name);

            if (exists)
            {
                throw new InvalidOperationException($"La uva '{newGrape.Name}' ya existe en el sistema.");
            }

            var grapeEntity = new Grape
            {
                Name = newGrape.Name.Trim(), 
            };

            await _grapeRepository.AddAsync(grapeEntity);
        }

        public async Task UpdateGrape(Guid id, UpdateGrapeRequest request)
        {
            if (_currentUser.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo administradores pueden editar uvas.");
            }

            var grape = await _grapeRepository.GetByIdAsync(id);
            if (grape == null)
            {
                throw new KeyNotFoundException($"No se encontró la uva con ID {id}");
            }

            var newNameNormalized = request.Name.Trim();

            if (!string.Equals(grape.Name, newNameNormalized, StringComparison.CurrentCultureIgnoreCase))
            {
                bool exists = await _grapeRepository.ExistsByNameAsync(newNameNormalized, id);

                if (exists)
                {
                    throw new InvalidOperationException($"Ya existe otra uva llamada '{request.Name}'.");
                }
            }

            grape.Name = newNameNormalized;
            grape.UpdatedAt = DateTime.Now;
            await _grapeRepository.UpdateAsync(grape);
        }

        public async Task DeleteGrape(Guid id)
        {
            if (_currentUser.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Solo administradores pueden editar uvas.");
            }

            var grape = await _grapeRepository.GetWithDetailsAsync(id);

            if (grape == null)
            {
                throw new KeyNotFoundException($"No se encontró la uva con ID {id}");
            }

            if (grape.WineGrapeVarieties != null && grape.WineGrapeVarieties.Any())
            {
                int winesCount = grape.WineGrapeVarieties.Count;

                throw new InvalidOperationException(
                    $"No se puede eliminar la uva '{grape.Name}' porque está asociada a {winesCount} vino(s). " +
                    "Por favor, elimina o desvincula los vinos relacionados antes de eliminar esta variedad.");
            }

            await _grapeRepository.DeleteAsync(grape); 
        }
    }
}
