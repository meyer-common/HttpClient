using Meyer.Common.HttpClient.Policies;
using Meyer.Common.HttpClient.Serializers;
using Meyer.Common.HttpClient.TokenProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Meyer.Common.HttpClient;

/// <summary>
/// Represents configurations for client behavior
/// </summary>
public class HttpClientOptions
{
    private ISerializer requestBodySerializer = new JsonSerializer();
    private ISerializer responseBodySerializer = new JsonSerializer();
    private IRetryPolicy retryPolicy = new NoRetryPolicy();

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
        set => retryPolicy = value ?? throw new ArgumentNullException("RetryPolicy");
    }

    /// <summary>
    /// Gets or sets a proxy to use when sending requests
    /// </summary>
    public IWebProxy Proxy { get; set; }

    /// <summary>
    /// Gets or sets whether to enable automatic decompression if the handler supports it
    /// </summary>
    public bool SupportsAutomaticDecompression { get; set; }

    /// <summary>
    /// Gets or sets how long the rest client will wait for a response when sending a request
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Gets or sets the serializer to use when parsing requests. Defaults to JsonSerializer
    /// </summary>
    public ISerializer RequestBodySerializer
    {
        get => requestBodySerializer;
        set => requestBodySerializer = value ?? throw new ArgumentNullException("RequestBodySerializer");
    }

    /// <summary>
    /// Gets or sets the serializer to use when parsing responses. Defaults to JsonSerializer
    /// </summary>
    public ISerializer ResponseBodySerializer
    {
        get => responseBodySerializer;
        set => responseBodySerializer = value ?? throw new ArgumentNullException("ResponseBodySerializer");
    }

    /// <summary>
    /// Gets or sets the level to log full request and responses. Defaults to None
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.None;

    /// <summary>
    /// Gets or sets whether to throw an HttpClientException when the response is an unsuccessful status code.
    /// Unlike the HttpClient, enabling this does not clear the HttpResponseMessage
    /// Defaults to true. Note: 404 is always considered successful for GET
    /// </summary>
    public bool EnsureSuccessStatusCode { get; set; } = true;
}