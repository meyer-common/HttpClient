using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient;

/// <summary>
/// Represents an implementation of a client for performing http requests against REST endpoints
/// </summary>
public class RestClient : IRestClient
{
    private readonly System.Net.Http.HttpClient httpClient;
    private readonly Logger logger;
    private readonly HttpClientOptions options;

    /// <summary>
    /// Instantiates a new instance of RestClient
    /// </summary>
    /// <param name="httpClient">A configured instance of HttpClient</param>
    /// <param name="options">Options to be used for making the requests</param>
    public RestClient(System.Net.Http.HttpClient httpClient, HttpClientOptions options)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Instantiates a new instance of RestClient
    /// </summary>
    /// <param name="httpClient">A configured instance of HttpClient</param>
    /// <param name="options">Options to be used for making the requests</param>
    /// <param name="logger">Logger for logging requests and responses</param>
    public RestClient(System.Net.Http.HttpClient httpClient, HttpClientOptions options, Logger logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Performs an Http GET request
    /// </summary>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<R>> HttpGet<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await options.RetryPolicy.Execute(async () => await Send<R>(new HttpRequestMessage(HttpMethod.Get, route), parameters, null, headers));
    }

    /// <summary>
    /// Performs an Http POST request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<object>> HttpPost<T>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await HttpPost<T, object>(route, body, parameters, headers);
    }

    /// <summary>
    /// Performs an Http POST request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<R>> HttpPost<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await options.RetryPolicy.Execute(async () => await Send<R>(new HttpRequestMessage(HttpMethod.Post, route), parameters, body, headers));
    }

    /// <summary>
    /// Performs an Http PUT request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<object>> HttpPut<T>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await HttpPut<T, object>(route, body, parameters, headers);
    }

    /// <summary>
    /// Performs an Http PUT request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<R>> HttpPut<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await options.RetryPolicy.Execute(async () => await Send<R>(new HttpRequestMessage(HttpMethod.Put, route), parameters, body, headers));
    }

    /// <summary>
    /// Performs an Http PATCH request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<object>> HttpPatch<T>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await HttpPatch<T, object>(route, body, parameters, headers);
    }

    /// <summary>
    /// Performs an Http PATCH request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<R>> HttpPatch<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await options.RetryPolicy.Execute(async () => await Send<R>(new HttpRequestMessage(new HttpMethod("PATCH"), route), parameters, body, headers));
    }

    /// <summary>
    /// Performs an Http DELETE request
    /// </summary>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<object>> HttpDelete(string route, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await HttpDelete<object>(route, parameters, headers);
    }

    /// <summary>
    /// Performs an Http DELETE request
    /// </summary>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    public async Task<HttpClientResponse<R>> HttpDelete<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        return await options.RetryPolicy.Execute(async () => await Send<R>(new HttpRequestMessage(HttpMethod.Delete, route), parameters, null, headers));
    }

    private async Task<HttpClientResponse<T>> Send<T>(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> parameters, object body,
        IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        request.RequestUri = new Uri($"{request.RequestUri}{parameters?.ToQueryString()}".Trim('/'), UriKind.Relative);

        await SetHeaders(request, headers);

        options.RequestBodySerializer.Serialize(body, request);

        if (options.LogLevel != LogLevel.None)
            await logger.LogRequest(options.LogLevel, request);

        var response = await httpClient.SendAsync(request);

        if (options.LogLevel != LogLevel.None)
        {
            await logger.LogResponse(options.LogLevel, response);
        }

        if (!response.IsSuccessStatusCode)
        {
            if (request.Method == HttpMethod.Get && response.StatusCode == HttpStatusCode.NotFound)
                return new HttpClientResponse<T>(response, default);

            if(!options.EnsureSuccessStatusCode)
                return new HttpClientResponse<T>(response, default);

            throw new HttpClientException(response);
        }

        try
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return new HttpClientResponse<T>(response, default);

            return new HttpClientResponse<T>(response, options.ResponseBodySerializer.Deserialize<T>(content));
        }
        catch (Exception e)
        {
            throw new HttpClientException(e, response);
        }
    }

    private async Task SetHeaders(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> headers = null)
    {
        if (options.TokenProvider != null)
        {
            var token = await options.TokenProvider.GetToken();

            if (string.IsNullOrWhiteSpace(token.Scheme))
                request.Headers.Add("Authorization", token.AccessToken);
            else
                request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }
    }
}