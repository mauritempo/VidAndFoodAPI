using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.Celllear
{
    public class CreateCellarRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        // Nuevo campo opcional
        public int? Capacity { get; set; }
    }
}
