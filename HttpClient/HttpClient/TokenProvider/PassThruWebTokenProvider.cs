using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider;

/// <summary>
/// Represents a token provider where the token is reused from the incoming request
/// </summary>
public class PassThruWebTokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Instantiates a new instance of PassThruWebTokenProvider
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context</param>
    public PassThruWebTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Returns a token from the incoming request
    /// </summary>
    /// <returns>Returns a token</returns>
    public async Task<TokenResponse> GetToken()
    {
        var headers = httpContextAccessor.HttpContext.Request.Headers;

        if (!headers.ContainsKey("Authorization"))
            throw new TokenProviderException("Authorization header not present on incoming request", null);

        return await Task.FromResult(new TokenResponse
        {
            //TODO: write better code
            AccessToken = headers["Authorization"][0]
                .TrimStart('B', 'e', 'a', 'r')
                .Trim()
        });
    }
}