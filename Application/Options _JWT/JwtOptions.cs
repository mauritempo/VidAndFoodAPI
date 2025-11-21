using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = "VidAndFood.Api";
        public string Audience { get; set; } = "VidAndFood.Client";
        public int ExpMinutes { get; set; } = 60;
        public string Secret { get; set; } = string.Empty;
    }
}
