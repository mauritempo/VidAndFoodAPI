using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.User
{
    public class UserWineStatusDto
    {
        public Guid WineId { get; set; }
        public bool IsInHistory { get; set; } // ¿Lo ha tomado?
        public bool IsFavorite { get; set; }  // ¿Es favorito?

        public int TimesConsumed { get; set; }
        public string? PersonalNotes { get; set; }
        public int? PersonalRating { get; set; }
    }
}
