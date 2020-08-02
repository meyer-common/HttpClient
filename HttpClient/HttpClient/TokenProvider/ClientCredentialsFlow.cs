using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider
{
    /// <summary>
    /// Represents a token provider for getting a token via client_credentials grant_type
    /// </summary>
    public class ClientCredentialsFlow : Flow, ITokenProvider
    {
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
        /// Instantiates a new instance of ClientCredentialsFlow
        /// </summary>
        /// <param name="tokenEndpoint">The base address to the idenetity server</param>
        /// <param name="clientId">The client id for the token request</param>
        /// <param name="clientSecret">The client secret for the token request</param>
        /// <param name="scopes">The requested scopes for the token request</param>
        public ClientCredentialsFlow(string tokenEndpoint, string clientId, string clientSecret, string scopes) : base(tokenEndpoint)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.Scopes = scopes;
        }

        /// <summary>
        /// Returns a token with the provided client and scopes
        /// </summary>
        /// <returns>Returns a token</returns>
        public async Task<Token> GetToken()
        {
            return await RequestToken(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", this.ClientId },
                { "client_secret", this.ClientSecret },
                { "scope", this.Scopes }
            });
        }
    }
}
