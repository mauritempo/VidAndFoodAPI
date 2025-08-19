using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CellarPhysics
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public int UserId { get; set; }  // dueño (sommelier)
        public User? User { get; set; }

        [Required] public string Name { get; set; } 
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<WineUserCellarItem> Items { get; set; } = new List<WineUserCellarItem>();
    }

}
