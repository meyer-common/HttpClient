using Polly;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.Policies;

/// <summary>
/// Represents a default implementation which does basic retry implementation
/// </summary>
public class DefaultRetryPolicy : IRetryPolicy
{
    private readonly int retryCount;

    /// <summary>
    /// Instantiates a new instance of DefaultRetryPolicy
    /// </summary>
    /// <param name="retryCount">Sets how many times the request should be attempted with an unsuccessful result</param>
    public DefaultRetryPolicy(int retryCount = 3)
    {
        this.retryCount = retryCount;
    }

    /// <summary>
    /// Executes the policy
    /// </summary>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="request">The request to wrap in policy</param>
    /// <returns>Returns the response once the policy has completed</returns>
    public async Task<HttpClientResponse<R>> Execute<R>(Func<Task<HttpClientResponse<R>>> request)
    {
        return await Policy
            .Handle<IOException>()
            .Or<WebException>(e =>
                e.Status == WebExceptionStatus.RequestCanceled ||
                e.Status == WebExceptionStatus.SendFailure ||
                e.Status == WebExceptionStatus.ConnectFailure ||
                e.Status == WebExceptionStatus.Pending)
            .Or<TaskCanceledException>()
            .Or<HttpClientException>(e =>
                e.HttpResponseMessage.StatusCode == HttpStatusCode.InternalServerError ||
                e.HttpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable ||
                e.HttpResponseMessage.StatusCode == HttpStatusCode.GatewayTimeout ||
                e.HttpResponseMessage.StatusCode == HttpStatusCode.TooManyRequests
            )
            .WaitAndRetryAsync(retryCount, retryAttempt =>
                TimeSpan.FromSeconds(retryAttempt)
            )
            .ExecuteAsync(request);
    }
}