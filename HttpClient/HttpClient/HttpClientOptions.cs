using Meyer.Common.HttpClient.Compression;
using Meyer.Common.HttpClient.Policies;
using Meyer.Common.HttpClient.TokenProvider;
using System;
using System.Net;

namespace Meyer.Common.HttpClient
{
    /// <summary>
    /// Represents configurations for the RestClient behavior
    /// </summary>
    public class HttpClientOptions
    {
        private IRetryPolicy retryPolicy = new NoRetryPolicy();

        /// <summary>
        /// Gets or sets the base absolute url
        /// </summary>
        public string BaseEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the provider used for token management
        /// </summary>
        public ITokenProvider TokenProvider { get; set; }

        /// <summary>
        /// Gets or sets the provider for retry and circuit breaker management. Defaults to none
        /// </summary>
        public IRetryPolicy RetryPolicy
        {
            get => retryPolicy;
            set => retryPolicy = value ?? throw new ArgumentNullException("value");
        }

        /// <summary>
        /// Gets or sets the compression when sending requests
        /// </summary>
        public ICompression Compression { get; set; }

        /// <summary>
        /// Gets or sets a proxy to use when sending requests
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Gets or sets how long the rest client will wait for a response when sending a request
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);
    }
}