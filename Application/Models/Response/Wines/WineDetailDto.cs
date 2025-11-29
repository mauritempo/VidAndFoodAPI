using Domain.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Wines
{
    public class WineDetailDto : WineListItemDto
    {
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // Necesitamos una lista de objetos para mostrar las uvas en el detalle
        public List<GrapeDto> Grapes { get; set; } = new();
    }
}
