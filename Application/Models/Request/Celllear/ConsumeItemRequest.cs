using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.Celllear
{
    public class ConsumeItemRequest
    {
        public Guid CellarId { get; set; }
        public Guid WineId { get; set; }
        public int Quantity { get; set; } // Cuántas botellas sacó (generalmente 1)
        public bool RegisterHistory { get; set; } = true; // ¿Lo bebió? (True) ¿O se rompió/regaló? (False)
        public string? Notes { get; set; } // Notas de cata opcionales si lo bebió
    }
}
