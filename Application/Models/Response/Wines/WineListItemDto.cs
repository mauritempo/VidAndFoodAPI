using Application.Models.Response.Rating;
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

        public string? NotesTaste { get; set; } = string.Empty;
        public string? Aroma { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public int VintageYear { get; set; }
        public string? ImageUrl { get; set; }
        public double AverageScore { get; set; }
        public List<GrapeResponseDto> Grapes { get; set; } = new();
        public bool IsActive { get; set; }
        public List<WineReviewDto> Reviews { get; set; }


    }
}
