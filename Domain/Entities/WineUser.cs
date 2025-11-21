using Domain.common;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WineUser : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }


        public string? TastingNotes { get; set; }
        public string? Opinion { get; set; }
        public bool  isCellarActive { get; set; } 


        public TypeCellar TypeCellar { get; set; }


        public virtual CellarPhysics? CellarPhysics { get; set; } 
        public virtual ICollection<WineUserCellarItem> CellarItems { get; set; } = new List<WineUserCellarItem>();
    }
}
