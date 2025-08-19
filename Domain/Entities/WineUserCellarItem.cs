using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WineUserCellarItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public int WineUserId { get; set; }
        public WineUser? WineUser { get; set; }

        [Required] public int CellarPhysicsId { get; set; }
        public CellarPhysics? CellarPhysics { get; set; }

        public int Quantity { get; set; }
        public decimal? PurchasePrice { get; set; } // (18,2)
        public string? LocationNote { get; set; }   // estante/posición

        // (WineId implícito vía WineUser → simplifica integridad con el usuario)
    }

}
