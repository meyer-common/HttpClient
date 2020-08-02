using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.TokenProvider
{
    /// <summary>
    /// Outlines methods for implementing a token provider
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Returns a token of the appropriate grant_type flow
        /// </summary>
        /// <returns>Returns a token</returns>
        Task<Token> GetToken();
    }
}