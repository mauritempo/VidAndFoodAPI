using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.Request
{
    public class RateWineRequest
    {
        public int Score { get; set; } // 1 a 5
        public string? Review { get; set; }
    }
}
