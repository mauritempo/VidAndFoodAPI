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
    public class WineUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }


        public string TastingNotes { get; set; }
        public string Opinion { get; set; }
        public bool  isCellarActive { get; set; } 


        public TypeCellar TypeCellar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }


        public virtual CellarPhysics CellarPhysics { get; set; } // Para la relación 1:1
        public virtual ICollection<WineUserCellarItem> CellarItems { get; set; } = new List<WineUserCellarItem>();
    }
}
