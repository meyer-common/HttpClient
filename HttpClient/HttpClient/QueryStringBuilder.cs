using System;
using System.Collections.Generic;
using System.Linq;

namespace Meyer.Common.HttpClient;

/// <summary>
/// Extension methods for an http query string
/// </summary>
public static class QueryStringBuilder
{
    /// <summary>
    /// Converts a collection of key value pairs to query string format
    /// </summary>
    /// <param name="parameters">A collection of key value pairs</param>
    /// <returns>The converted string</returns>
    public static string ToQueryString(this IEnumerable<KeyValuePair<string, string>> parameters)
    {
        if (parameters == null)
            return string.Empty;

        return "?" + string.Join("&", parameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}")).Trim('&');
    }
}