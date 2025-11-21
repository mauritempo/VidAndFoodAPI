using Domain.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Grape : BaseEntity
    {
        
        [Required]
        public string Name { get; set; }

        public virtual ICollection<WineGrapeVariety> WineGrapeVarieties { get; set; } = new List<WineGrapeVariety>(); // <-- Agregá esta línea

    }
}
