using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.Wines
{
    public class CreateWineRequest
    {
        public string Name { get; set; } = string.Empty;
        public string WineryName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public int VintageYear { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<Guid> Grapes { get; set; } = new(); // IDs de las uvas
    }

}
}
