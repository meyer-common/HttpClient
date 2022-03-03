using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient;

/// <summary>
/// Interface outlines methods for performing http requests against REST endpoints
/// </summary>
public interface IRestClient
{
    /// <summary>
    /// Performs an Http GET request
    /// </summary>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    Task<HttpClientResponse<R>> HttpGet<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

    /// <summary>
    /// Performs an Http POST request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    Task<HttpClientResponse<object>> HttpPost<T>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

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
    Task<HttpClientResponse<R>> HttpPost<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

    /// <summary>
    /// Performs an Http PUT request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    Task<HttpClientResponse<object>> HttpPut<T>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

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
    Task<HttpClientResponse<R>> HttpPut<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

    /// <summary>
    /// Performs an Http PATCH request
    /// </summary>
    /// <typeparam name="T">The type of the request body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="body">The body of the request</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    Task<HttpClientResponse<object>> HttpPatch<T>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

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
    Task<HttpClientResponse<R>> HttpPatch<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

    /// <summary>
    /// Performs an Http DELETE request
    /// </summary>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    Task<HttpClientResponse<object>> HttpDelete(string route, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);

    /// <summary>
    /// Performs an Http DELETE request
    /// </summary>
    /// <typeparam name="R">The type of the response body</typeparam>
    /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
    /// <param name="parameters">Optional query parameters to add to the request</param>
    /// <param name="headers">Optional headers to add to the request</param>
    /// <returns>Returns the parsed body as type R</returns>
    Task<HttpClientResponse<R>> HttpDelete<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null,
        IEnumerable<KeyValuePair<string, string>> headers = null);
}