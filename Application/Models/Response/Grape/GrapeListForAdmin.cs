using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Grape
{
    public class GrapeListForAdmin
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid Id { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
