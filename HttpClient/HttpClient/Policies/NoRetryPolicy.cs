using System;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.Policies
{
    /// <summary>
    /// Represents a default implementation which does not do any retry or circuit breaker implementation
    /// </summary>
    public class NoRetryPolicy : IRetryPolicy
    {
        /// <summary>
        /// Executes the policy
        /// </summary>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="request">The request to wrap in policy</param>
        /// <returns>Returns the response once the policy has completed</returns>
        public async Task<HttpClientResponse<R>> Execute<R>(Func<Task<HttpClientResponse<R>>> request)
        {
            return await request();
        }
    }
}