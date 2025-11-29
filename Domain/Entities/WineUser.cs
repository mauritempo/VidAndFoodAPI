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
        public Guid UserId { get; set; }
        public User User { get; set; }


        [Required]
        public Guid WineId { get; set; }
        public Wine Wine { get; set; }


        public string? TastingNotes { get; set; }
        public int TimesConsumed { get; set; } // Contador de veces bebido
        public DateTime? LastConsumedAt { get; set; } // Cuándo fue la última vez

    }
}
