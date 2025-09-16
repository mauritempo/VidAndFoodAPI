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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WineUserId { get; set; }
        public WineUser WineUser { get; set; }


        [Required] public string? Name { get; set; } 
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }

}
