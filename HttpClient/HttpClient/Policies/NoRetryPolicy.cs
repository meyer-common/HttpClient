﻿using Meyer.Common.HttpClient.Policies;
using System;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient
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
        public async Task<RestClientResponse<R>> Execute<R>(Func<Task<RestClientResponse<R>>> request)
        {
            return await request();
        }
    }
}