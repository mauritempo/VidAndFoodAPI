using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WineGrapeVariety
    {
        public int GrapeId { get; set; }
        public int WineId { get; set; }
        public Wine Wine { get; set; } 
        
        public Grape Grape { get; set; }
         
        public int? Percentage { get; set; }

    }
}
