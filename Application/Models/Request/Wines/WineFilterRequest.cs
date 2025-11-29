using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.Wines
{
    public class WineFilterRequest
    {
        public string? Term { get; set; }     // Búsqueda por texto (nombre, bodega)
        public Guid? GrapeId { get; set; }    // Filtro por uva
        public string? Region { get; set; }   // Filtro por región
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
