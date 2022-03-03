using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider;

/// <summary>
/// Represents a token provider for getting a token via client_credentials grant_type
/// </summary>
public class ClientCredentialsTokenProvider : Flow, ITokenProvider
{
    /// <summary>
    /// Instantiates a new instance of ClientCredentialsTokenProvider
    /// </summary>
    /// <param name="tokenEndpoint">The base address to the idenetity server</param>
    /// <param name="clientId">The client id for the token request</param>
    /// <param name="clientSecret">The client secret for the token request</param>
    /// <param name="scopes">The requested scopes for the token request</param>
    public ClientCredentialsTokenProvider(string tokenEndpoint, string clientId, string clientSecret, string scopes) :
        base(tokenEndpoint)
    {
        if (string.IsNullOrWhiteSpace(tokenEndpoint))
            throw new ArgumentException($"'{nameof(tokenEndpoint)}' cannot be null or empty.", nameof(tokenEndpoint));

        if (string.IsNullOrWhiteSpace(clientId))
            throw new ArgumentException($"'{nameof(clientId)}' cannot be null or empty.", nameof(clientId));

        if (string.IsNullOrWhiteSpace(clientSecret))
            throw new ArgumentException($"'{nameof(clientSecret)}' cannot be null or empty.", nameof(clientSecret));

        ClientId = clientId;
        ClientSecret = clientSecret;
        Scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
    }

    /// <summary>
    /// Gets the client id for the token request
    /// </summary>
    public string ClientId { get; }

    /// <summary>
    /// Gets the client secret for the token request
    /// </summary>
    public string ClientSecret { get; }

    /// <summary>
    /// Gets the requested scopes for the token request
    /// </summary>
    public string Scopes { get; }

    /// <summary>
    /// Returns a token with the provided client and scopes
    /// </summary>
    /// <returns>Returns a token</returns>
    public async Task<TokenResponse> GetToken()
    {
        return await RequestToken(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "scope", Scopes }
        });
    }
}