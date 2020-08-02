using System;
using System.Net;
using System.Net.Http;

namespace Meyer.Common.HttpClient
{
    internal static class HttpClientFactory
    {
        internal static System.Net.Http.HttpClient CreateHttpClientFrom(HttpClientOptions options)
        {
            var handler = new HttpClientHandler();

            if (options.Proxy != null && handler.SupportsProxy)
                handler.Proxy = options.Proxy;

            if (options.Compression != null && handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return new System.Net.Http.HttpClient(handler)
            {
                BaseAddress = new Uri(options.BaseEndpoint + "/"),
                Timeout = options.Timeout
            };
        }
    }
}