using System;

namespace Meyer.Common.HttpClient.TokenProvider;

/// <summary>
/// Represents an exception from a rest client request
/// </summary>
public class TokenProviderException : Exception
{
    /// <summary>
    /// Instantiates a new instance of TokenProviderException
    /// </summary>
    /// <param name="message">Message of the exception</param>
    /// <param name="e">Inner exception</param>
    public TokenProviderException(string message, Exception e) : base(message, e) { }
}