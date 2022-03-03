using System;
using System.Net.Http;

namespace Meyer.Common.HttpClient;

/// <summary>
/// Represents an exception from a rest client request
/// </summary>
public class HttpClientException : Exception
{
    /// <summary>
    /// Gets the full response information
    /// </summary>
    public HttpResponseMessage HttpResponseMessage { get; }

    /// <summary>
    /// Instantiates a new instance of HttpClientException
    /// </summary>
    /// <param name="e">The inner exception</param>
    /// <param name="httpResponseMessage">The full response object</param>
    public HttpClientException(Exception e, HttpResponseMessage httpResponseMessage) : base("See inner exception", e)
    {
        HttpResponseMessage = httpResponseMessage;
    }

    /// <summary>
    /// Instantiates a new instance of HttpClientException with no inner exception
    /// </summary>
    /// <param name="httpResponseMessage">The full response object</param>
    public HttpClientException(HttpResponseMessage httpResponseMessage) : base($"HTTP request error: {httpResponseMessage.StatusCode}")
    {
        HttpResponseMessage = httpResponseMessage;
    }
}