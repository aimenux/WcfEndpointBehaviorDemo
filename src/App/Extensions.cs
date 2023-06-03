using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace App;

public static class Extensions
{
    public static IHttpClientBuilder AddRetryPolicyHandler(this IHttpClientBuilder builder)
    {
        var retryPolicy = GetRetryPolicy();
        return builder.AddPolicyHandler(retryPolicy);
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int maxRetry = 3)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(maxRetry, ComputeDuration, (response, timeSpan, retryCount, context) =>
            {
                OnRetry(response, retryCount, maxRetry);
            });
    }

    private static void OnRetry(DelegateResult<HttpResponseMessage> response, int retryCount, int maxRetry)
    {
        var exception = response.Exception;
        if (exception != null)
        {
            var reason = exception.Message;
            Console.WriteLine($"Request failed with exception [{reason}]'. Retry attempt [{retryCount}/{maxRetry}].");
        }
        else
        {
            var statusCode = response.Result.StatusCode;
            Console.WriteLine($"Request failed with status code [{statusCode}]. Retry attempt [{retryCount}/{maxRetry}].");
        }
    }

    private static TimeSpan ComputeDuration(int retryCount) => TimeSpan.FromSeconds(Math.Pow(2, retryCount));
}