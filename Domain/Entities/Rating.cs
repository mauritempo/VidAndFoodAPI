using Domain.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating : BaseEntity
    {
        public Guid UserUuId { get; set; }  
        public Guid WineUuId { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }
        public bool IsPublic { get; set; } = true;

        [MaxLength(2000)] // Limitamos a 2000 caracteres para proteger la BD
        public string? Review { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User User { get; set; }

        public Wine Wine { get; set; }

    }
}
