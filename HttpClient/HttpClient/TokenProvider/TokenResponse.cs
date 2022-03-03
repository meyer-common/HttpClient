using System;
using System.Text.Json.Serialization;

namespace Meyer.Common.HttpClient.TokenProvider;

/// <summary>
/// Represents a container for authentication
/// </summary>
public class TokenResponse
{
    private long expiresIn;

    /// <summary>
    /// Gets or sets the identity token
    /// </summary>
    [JsonPropertyName("identity_token")]
    public string IdentityToken { get; set; }

    /// <summary>
    /// Gets or sets the access token
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the number of seconds until the token expires from the time of retrieval
    /// </summary>
    [JsonPropertyName("expires_in")]
    public long ExpiresIn
    {
        get => expiresIn;
        set
        {
            Expires = DateTimeOffset.UtcNow.AddSeconds(value);
            expiresIn = value;
        }
    }

    /// <summary>
    /// Gets the time when the token will expire based on ExpiresIn property
    /// </summary>
    public DateTimeOffset Expires { get; private set; }

    /// <summary>
    /// Gets or sets the auth scheme
    /// </summary>
    public string Scheme { get; set; } = string.Empty;

    /// <summary>
    /// Checks whether the token has expired or is close to expiring
    /// </summary>
    /// <returns>Returns true if the token is within 90 seconds of expiring</returns>
    public bool IsExpired()
    {
        return (Expires - DateTime.UtcNow).TotalSeconds <= 90;
    }
}