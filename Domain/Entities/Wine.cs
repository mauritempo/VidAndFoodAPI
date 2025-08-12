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
        public required string Name { get; set; }
        [Required]
        public string Region { get; set; }

        [Required]
        public Enum WyneType { get; set; }

        public int VintageYear { get; set; }
        public string? LabelImageUrl { get; set; }
        public string? TastingNotes { get; set; }

        public string? Aroma { get; set; }

        

        public WineGrapeVariety { get; set; }
        public WineTypeId  { get; set; }
        public WineryIid  { get; set; }
    }
}
