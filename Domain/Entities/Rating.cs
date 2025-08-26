using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int UserId { get; set; }  
        public int WineId { get; set; }

        public int Rate { get; set; } 
        public User User { get; set; }

        public Wine Wine { get; set; }

    }
}
