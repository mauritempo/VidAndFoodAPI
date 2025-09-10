using Domain.Entities.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Wine 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string WineryName { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }


        [Required]
        public WineType WineType { get; set; }

        public int VintageYear { get; set; }
        public string? LabelImageUrl { get; set; }
        public string? TastingNotes { get; set; }

        public string? Aroma { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<WineGrapeVariety> WineGrapeVarieties { get; set; } = new List<WineGrapeVariety>();
        public virtual ICollection<WineFavorite> FavoritedByUsers { get; set; } = new List<WineFavorite>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<WineUserCellarItem> CellarItems { get; set; } = new List<WineUserCellarItem>();
    }
}
