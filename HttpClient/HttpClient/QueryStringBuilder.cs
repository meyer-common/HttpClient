using System;
using System.Collections.Generic;

namespace Meyer.Common.HttpClient
{
    internal static class QueryStringBuilder
    {
        internal static string ToQueryString(this IEnumerable<KeyValuePair<string, string>> parameters)
        {
            string toReturn = string.Empty;
            if (parameters != null)
            {
                var counter = 0;
                foreach (var queryParameter in parameters)
                {
                    if (counter == 0)
                    {
                        toReturn += $"?{queryParameter.Key}={Uri.EscapeDataString(queryParameter.Value)}";
                    }
                    else
                    {
                        toReturn += $"&{queryParameter.Key}={Uri.EscapeDataString(queryParameter.Value)}";
                    }

                    counter++;
                }
            }

            return toReturn;
        }
    }
}
