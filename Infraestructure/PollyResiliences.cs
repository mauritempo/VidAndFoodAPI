using Polly;
using Polly.Extensions.Http;
using System.Net;


namespace Infrastructure
{
    public static class PollyResiliencePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ApiClientConfiguration apiClientConfiguration)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.BadRequest)
                
                .WaitAndRetryAsync(
                    apiClientConfiguration.RetryCount
                    ,attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt) * apiClientConfiguration.RetryAttemptInSeconds) // backoff exponencial
                );
        }


        /// <summary>
        /// Devuelve una política de Circuit Breaker que abre el circuito
        /// después de 3 fallos y lo mantiene 30s antes de reintentar.
        /// </summary>
        /// <returns>Política de resiliencia con Circuit Breaker</returns>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ApiClientConfiguration apiClientConfiguration)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.BadRequest)
                .CircuitBreakerAsync(
                    apiClientConfiguration.HandleEventsAllowedBeforeBreaking,
                    durationOfBreak: TimeSpan.FromMinutes(apiClientConfiguration.DurationOfBreakInSeconds)
                );
        }
    }
}
