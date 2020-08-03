using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider
{
    /// <summary>
    /// Represents a token provider for getting a token via password grant_type
    /// </summary>
    public class ResourceOwnerTokenProvider : Flow, ITokenProvider
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
        /// Gets the username for the token request
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the password for the user for the token request
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the requested scopes for the token request
        /// </summary>
        public string Scopes { get; }

        /// <summary>
        /// Instantiates a new instance of ResourceOwnerTokenProvider
        /// </summary>
        /// <param name="tokenEndpoint">The address to the token server</param>
        /// <param name="clientId">The client id for the token request</param>
        /// <param name="clientSecret">The client secret for the token request</param>
        /// <param name="username">The username for the token request</param>
        /// <param name="password">The password for the user for the token request</param>
        /// <param name="scopes">The requested scopes for the token request</param>
        public ResourceOwnerTokenProvider(string tokenEndpoint, string clientId, string clientSecret, string username, string password, string scopes) : base(tokenEndpoint)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.Username = username;
            this.Password = password;
            this.Scopes = scopes;
        }

        /// <summary>
        /// Returns a token with the provided client, user, and scopes
        /// </summary>
        /// <returns>Returns a token</returns>
        public async Task<Token> GetToken()
        {
            return await RequestToken(new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", this.ClientId },
                { "client_secret", this.ClientSecret },
                { "username", this.Username },
                { "password", this.Password },
                { "scope", this.Scopes }
            });
        }
    }
}