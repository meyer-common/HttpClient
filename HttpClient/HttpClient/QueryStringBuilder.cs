using System;
using System.Collections.Generic;
using System.Linq;

namespace Meyer.Common.HttpClient;

public static class QueryStringBuilder
{
    public static string ToQueryString(this IEnumerable<KeyValuePair<string, string>> parameters)
    {
        if (parameters == null)
            return string.Empty;

        return "?" + string.Join("&", parameters
                .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"))
            .Trim('&');
    }
}