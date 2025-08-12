using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WineGrapeVariety
    {
        public int WineId { get; set; }
        public Wine Wine { get; set; } = null!;
        public int GrapeVarietyId { get; set; }
        public Grape GrapeVariety { get; set; } = null!;
         
        public int? percentage { get; set; }
    }
}
