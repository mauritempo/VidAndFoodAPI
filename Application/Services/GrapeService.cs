using Application.Interfaces;
using Application.Models.Response;
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

        public GrapeService (IGrapeRepository grapeRepository)
        {
            _grapeRepository = grapeRepository;
        }
        public async Task<List<GrapeResponseDto>> GetAllGrapes()
        {
            var grapes = await _grapeRepository.GetAll();
            
            return grapes.Select(g => new GrapeResponseDto
            {
                Id = g.UuId,   // Asumiendo que tu DTO tiene estas propiedades
                Name = g.Name
            }).ToList();
        }
    }
}
