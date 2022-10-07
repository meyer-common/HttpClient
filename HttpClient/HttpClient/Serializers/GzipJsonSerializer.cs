using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Meyer.Common.HttpClient.Serializers;

/// <summary>
/// Represents a serializer for compressed content
/// </summary>
public class GzipJsonSerializer : ISerializer
{
    /// <summary>
    /// Deserializes response body from application/json
    /// </summary>
    /// <typeparam name="T">The type to deserialize to</typeparam>
    /// <param name="body">The body of the content</param>
    /// <returns>Response body as type T</returns>
    public T Deserialize<T>(string body)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(body);
    }

    /// <summary>
    /// Serializes request body to application/json with content encoding gzip
    /// </summary>
    /// <param name="body">The body to serialize</param>
    /// <param name="request">The pending http request message</param>
    public void Serialize(object body, HttpRequestMessage request)
    {
        if (body == null) return;

        var content = System.Text.Json.JsonSerializer.Serialize(body, new JsonSerializerOptions
        { 
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        });

        using var dataStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        using var compressed = new MemoryStream();
        using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
        {
            gzip.Write(dataStream.ToArray(), 0, (int)dataStream.Length);
        }

        request.Content = new ByteArrayContent(compressed.ToArray());
        request.Content.Headers.Add("Content-Encoding", "gzip");
    }
}