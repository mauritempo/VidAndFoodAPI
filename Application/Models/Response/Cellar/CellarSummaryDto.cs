using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Cellar
{
    public class CellarSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Cantidad actual de botellas (Calculado en el servicio)
        public int TotalBottles { get; set; }

        // Capacidad máxima (null = sin límite)
        // Útil para que el frontend calcule el % de ocupación
        public int? Capacity { get; set; }
    }
}
