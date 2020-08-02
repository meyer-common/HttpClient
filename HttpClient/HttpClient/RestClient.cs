using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient
{
    /// <summary>
    /// Represents an implementation of a client for performing http requests against REST endpoints
    /// </summary>
    public class RestClient : IRestClient
    {
        private System.Net.Http.HttpClient client;
        private RestClientOptions options;

        /// <summary>
        /// Instantiates a new instance of RestClient
        /// </summary>
        /// <param name="options">Options to be used for making the requests</param>
        public RestClient(RestClientOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

            var handler = new HttpClientHandler();

            if (options.Proxy != null && handler.SupportsProxy)
                handler.Proxy = options.Proxy;

            if (options.UseGzipCompression && handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            this.client = new System.Net.Http.HttpClient(handler)
            {
                BaseAddress = new Uri(options.BaseEndpoint + "/"),
                Timeout = options.Timeout
            };
        }

        /// <summary>
        /// Performs an Http GET request
        /// </summary>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        public async Task<RestClientResponse<R>> HttpGet<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return await this.options.RetryPolicy.Execute(async () => await this.Send<R>(new HttpRequestMessage(HttpMethod.Get, route), parameters, null, headers));
        }

        /// <summary>
        /// Performs an Http POST request
        /// </summary>
        /// <typeparam name="T">The type of the request body</typeparam>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="body">The body of the request</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        public async Task<RestClientResponse<R>> HttpPost<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return await this.options.RetryPolicy.Execute(async () => await this.Send<R>(new HttpRequestMessage(HttpMethod.Post, route), parameters, body, headers));
        }

        /// <summary>
        /// Performs an Http PUT request
        /// </summary>
        /// <typeparam name="T">The type of the request body</typeparam>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="body">The body of the request</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        public async Task<RestClientResponse<R>> HttpPut<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return await this.options.RetryPolicy.Execute(async () => await this.Send<R>(new HttpRequestMessage(HttpMethod.Put, route), parameters, body, headers));
        }

        /// <summary>
        /// Performs an Http PATCH request
        /// </summary>
        /// <typeparam name="T">The type of the request body</typeparam>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="body">The body of the request</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        public async Task<RestClientResponse<R>> HttpPatch<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return await this.options.RetryPolicy.Execute(async () => await this.Send<R>(new HttpRequestMessage(new HttpMethod("PATCH"), route), parameters, body, headers));
        }

        /// <summary>
        /// Performs an Http DELETE request
        /// </summary>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        public async Task<RestClientResponse<R>> HttpDelete<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return await this.options.RetryPolicy.Execute(async () => await this.Send<R>(new HttpRequestMessage(HttpMethod.Delete, route), parameters, null, headers));
        }

        private async Task<RestClientResponse<T>> Send<T>(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> parameters, object body, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            request.RequestUri = new Uri($"{request.RequestUri}/{this.BuildQueryString(parameters)}".Trim('/'), UriKind.Relative);

            await this.SetHeaders(request, headers);

            this.SetBody(request, body);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new RestClientResponse<T>(response, default(T));

                throw new RestClientException(response);
            }

            try
            {
                return new RestClientResponse<T>(response, JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()));
            }
            catch (Exception e)
            {
                throw new RestClientException(e, response);
            }
        }

        private string BuildQueryString(IEnumerable<KeyValuePair<string, string>> parameters)
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

        private async Task SetHeaders(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (this.options.TokenProvider != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", (await this.options.TokenProvider.GetToken()).AccessToken);

            if (headers != null)
            {
                foreach (var header in headers)
                    request.Headers.Add(header.Key, header.Value);
            }
        }

        private void SetBody(HttpRequestMessage request, object body)
        {
            if (body != null)
            {
                var jsonContent = JsonConvert.SerializeObject(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                if (this.options.UseGzipCompression)
                {
                    request.Content = new ByteArrayContent(GzipCompress(jsonContent));
                    request.Content.Headers.Add("Content-Encoding", "gzip");
                }
                else
                {
                    request.Content = new StringContent(jsonContent);
                }
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
        }

        private static byte[] GzipCompress(string data)
        {
            using (var dataStream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                using (var compressed = new MemoryStream())
                {
                    using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
                    {
                        gzip.Write(dataStream.ToArray(), 0, (int)dataStream.Length);
                    }

                    return compressed.ToArray();
                }
            }
        }
    }
}