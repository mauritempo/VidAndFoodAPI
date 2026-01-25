using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class PollyResiliencePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var jitter = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(2, retryAttempt => new TimeSpan(0, 0, 4));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
           HttpPolicyExtensions
               .HandleTransientHttpError()
               .OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests)
               .CircuitBreakerAsync(
                   handledEventsAllowedBeforeBreaking: 5,
                   durationOfBreak: TimeSpan.FromSeconds(30)
               );
    }
}
