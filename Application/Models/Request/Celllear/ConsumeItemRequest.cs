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
        public int Quantity { get; set; } 
        public bool RegisterHistory { get; set; } = true; 
        public string? Notes { get; set; } 
    }
}
