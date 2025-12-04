using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.Celllear
{
    public class AddWineToCellarRequest
    {
        [Required]
        public Guid CellarId { get; set; }

        [Required]
        public Guid WineId { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Quantity { get; set; }

        public decimal? PurchasePrice { get; set; }
    }
}
