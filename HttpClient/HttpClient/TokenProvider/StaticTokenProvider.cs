using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider;

/// <summary>
/// Represents a token provider for a static value
/// </summary>
public class StaticTokenProvider : ITokenProvider
{
    private TokenResponse token;

    /// <summary>
    /// Returns a token with the provided client and scopes
    /// </summary>
    /// <returns>Returns a token</returns>
    public Task<TokenResponse> GetToken()
    {
        return Task.FromResult(token);
    }

    /// <summary>
    /// Sets a static token to be used by the token provider
    /// </summary>
    /// <param name="token">The static token</param>
    public void SetToken(TokenResponse token)
    {
        this.token = token;
    }
}