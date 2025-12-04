using Domain.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WineFavorite : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid WineId { get; set; }

        public User User { get; set; } = null!;
        public Wine Wine { get; set; } = null!;
    }
}
