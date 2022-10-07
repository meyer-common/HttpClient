using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Meyer.Common.HttpClient.Serializers;

/// <summary>
/// Represents a serializer for application/json
/// </summary>
public class JsonSerializer : ISerializer
{
    /// <summary>
    /// Deserializes response body from application/json
    /// </summary>
    /// <typeparam name="T">The type to deserialize to</typeparam>
    /// <param name="body">The body of the content</param>
    /// <returns>Response body as type T</returns>
    public T Deserialize<T>(string body)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    /// <summary>
    /// Serializes request body to application/json
    /// </summary>
    /// <param name="body">The body to serialize</param>
    /// <param name="request">The pending http request message</param>
    public virtual void Serialize(object body, HttpRequestMessage request)
    {
        if (body == null) return;

        var content = System.Text.Json.JsonSerializer.Serialize(body, new JsonSerializerOptions 
        { 
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        });

        request.Content = new StringContent(content);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
    }
}