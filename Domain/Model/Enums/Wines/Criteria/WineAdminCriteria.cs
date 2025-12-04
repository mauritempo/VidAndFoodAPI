using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Enums.Wines.Criteria
{
        public class WineAdminCriteria
        {
            public string? SearchTerm { get; set; }
            public bool? ShowDeleted { get; set; } // Propiedad exclusiva de admin
        }
}
