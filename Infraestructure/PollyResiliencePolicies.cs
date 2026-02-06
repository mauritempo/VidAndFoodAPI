using Polly;
using Polly.Extensions.Http;
using System.Net;
using Infrastructure.Services.Resilience; // Asegúrate de que esto esté aquí

namespace Infrastructure
{
    public static class PollyResiliencePolicies
    {
        // Ahora acepta el objeto ApiClientConfiguration
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ApiClientConfiguration config)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(
                    config.RetryCount, 
                    _ => TimeSpan.FromSeconds(config.RetryAttemptInSeconds)
                );
        }

        // Ahora acepta el objeto ApiClientConfiguration
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ApiClientConfiguration config) =>
           HttpPolicyExtensions
               .HandleTransientHttpError()
               .OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests)
               .CircuitBreakerAsync(
                   handledEventsAllowedBeforeBreaking: config.HandleEventsAllowedBeforeBreaking,
                   durationOfBreak: TimeSpan.FromSeconds(config.DurationOfBreakInSeconds)
               );
    }
}