using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.common;

namespace Domain.Entities
{
    public class CellarPhysics: BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string? Name { get; set; }

        public int? Capacity { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<WineUserCellarItem> Items { get; set; } = new List<WineUserCellarItem>();
    }

}

