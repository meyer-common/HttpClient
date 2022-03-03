using System.Net.Http;

namespace Meyer.Common.HttpClient.Serializers;

/// <summary>
/// Interface outlines methods for serializing and deserializing strategies for an http request
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Deserializes response body
    /// </summary>
    /// <typeparam name="T">The type to deserialize to</typeparam>
    /// <param name="body">The body of the content</param>
    /// <returns>Response body as type T</returns>
    T Deserialize<T>(string body);

    /// <summary>
    /// Serializes request body to application/json
    /// </summary>
    /// <param name="body">The body to serialize</param>
    /// <param name="request">The pending http request message</param>
    void Serialize(object body, HttpRequestMessage request);
}