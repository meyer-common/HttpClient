using Newtonsoft.Json;
using System;

namespace Meyer.Common.HttpClient.TokenProvider
{
    /// <summary>
    /// Represents a container for authentication
    /// </summary>
    public class Token
    {
        private long expiresIn;

        /// <summary>
        /// Gets or sets the identity token
        /// </summary>
        [JsonProperty("identity_token")]
        public string IdentityToken { get; set; }

        /// <summary>
        /// Gets or sets the access token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds until the token expires from the time of retrieval
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn
        {
            get { return this.expiresIn; }
            set
            {
                this.Expires = DateTimeOffset.UtcNow.AddSeconds(value);
                this.expiresIn = value;
            }
        }

        /// <summary>
        /// Gets the time when the token will expire based on ExpiresIn property
        /// </summary>
        public DateTimeOffset Expires { get; private set; }

        /// <summary>
        /// Gets or sets the optional refresh token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Checks whether the token has expired or is close to expiring
        /// </summary>
        /// <returns>Returns true if the token is within 90 seconds of expiring</returns>
        public bool IsExpired()
        {
            return (this.Expires - DateTime.UtcNow).TotalSeconds <= 90;
        }
    }
}