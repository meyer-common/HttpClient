using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Meyer.Common.HttpClient.Serializers;

/// <summary>
/// Represents a serializer for application/x-www-form-urlencoded
/// </summary>
public class FormUrlEncodedSerializer : ISerializer
{
    /// <summary>
    /// Not implemented
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="body"></param>
    /// <returns></returns>
    public T Deserialize<T>(string body)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Serializes request body to application/x-www-form-urlencoded
    /// </summary>
    /// <param name="body">The body to serialize</param>
    /// <param name="request">The pending http request message</param>
    public void Serialize(object body, HttpRequestMessage request)
    {
        if (body == null) return;

        var json = System.Text.Json.JsonSerializer.Serialize(body, new JsonSerializerOptions
        { 
            NumberHandling = JsonNumberHandling.WriteAsString 
        });
        var dictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        request.Content = new FormUrlEncodedContent(dictionary);
    }
}