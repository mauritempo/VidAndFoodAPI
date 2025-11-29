using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Resilience
{
    public class ApiClientConfiguration
    {
        public int RetryCount { get; set; }
        public int RetryAttemptInSeconds { get; set; }
        public int HandleEventsAllowedBeforeBreaking { get; set; }
        public int DurationOfBreakInSeconds { get; set; }
    }
}
