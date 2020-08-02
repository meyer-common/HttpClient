using System;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.Policies
{
    /// <summary>
    /// Outlines methods for implementing a retry/circuit breaker policy for http requests
    /// </summary>
    public interface IRetryPolicy
    {
        /// <summary>
        /// Executes the policy
        /// </summary>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="request">The request to wrap in policy</param>
        /// <returns>Returns the response once the policy has completed</returns>
        Task<HttpClientResponse<R>> Execute<R>(Func<Task<HttpClientResponse<R>>> request);
    }
}