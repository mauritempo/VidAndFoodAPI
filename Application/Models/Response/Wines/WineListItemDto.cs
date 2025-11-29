using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Wines
{
    public class WineListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string WineryName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VintageYear { get; set; }
        public string? ImageUrl { get; set; }
        public double AverageScore { get; set; }
        public string GrapeNames { get; set; } = string.Empty; // Texto plano para la lista
    }
}
