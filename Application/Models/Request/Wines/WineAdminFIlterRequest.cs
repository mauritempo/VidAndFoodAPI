using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.Wines
{
    public class WineAdminFilterRequest
    {
        public string? SearchTerm { get; set; }
        public bool? ShowDeleted { get; set; }
    }
}
