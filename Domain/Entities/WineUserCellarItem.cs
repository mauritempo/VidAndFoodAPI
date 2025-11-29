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
    public class WineUserCellarItem : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Required]
        public Guid CellarPhysicsId { get; set; }
        public virtual CellarPhysics CellarPhysics { get; set; }


        [Required]
        public Guid WineId { get; set; }
        public virtual Wine Wine { get; set; }


        public int Quantity { get; set; }
        public string? LocationNote { get; set; }
        public decimal? PurchasePrice { get; set; } 
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    }

}
