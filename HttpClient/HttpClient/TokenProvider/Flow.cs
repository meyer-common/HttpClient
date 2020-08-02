using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider
{
    public abstract class Flow
    {
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        protected Uri tokenEndpoint;

        public Token Token { get; private set; }

        public Flow(string tokenEndpoint)
        {
            this.tokenEndpoint = new Uri(tokenEndpoint);
        }

        protected async Task<Token> RequestToken(Dictionary<string, string> headerDictionary)
        {
            if (this.Token != null && !this.Token.IsExpired())
                return this.Token;

            await semaphore.WaitAsync();

            try
            {
                if (this.Token != null && !this.Token.IsExpired())
                    return this.Token;

                using (var client = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(60), BaseAddress = this.tokenEndpoint })
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
                    request.Headers.Add("cache-control", "no-cache");

                    request.Content = new FormUrlEncodedContent(headerDictionary);
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();

                    this.Token = JsonConvert.DeserializeObject<Token>(responseContent, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    this.Token.Scheme = "Bearer";

                    return this.Token;
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}