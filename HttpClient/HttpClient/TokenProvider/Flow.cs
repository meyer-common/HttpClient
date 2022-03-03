using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider;

public abstract class Flow
{
    private static readonly SemaphoreSlim semaphore = new(1);

    protected Uri tokenEndpoint;

    protected Flow(string tokenEndpoint)
    {
        this.tokenEndpoint = new Uri(tokenEndpoint);
    }

    public TokenResponse Token { get; private set; }

    protected async Task<TokenResponse> RequestToken(Dictionary<string, string> contentDictionary)
    {
        if (Token != null && !Token.IsExpired())
            return Token;

        await semaphore.WaitAsync();

        try
        {
            if (Token != null && !Token.IsExpired())
                return Token;

            using var client = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(60) };

            var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
            request.Headers.Add("cache-control", "no-cache");

            request.Content = new FormUrlEncodedContent(contentDictionary);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            Token = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });

            Token.Scheme = "Bearer";

            return Token;
        }
        catch (Exception e)
        {
            throw new TokenProviderException(
                "Provider failed to get token for request. See innter exception for details", e);
        }
        finally
        {
            semaphore.Release();
        }
    }
}