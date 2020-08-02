using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient
{
    public class FileClient
    {
        private System.Net.Http.HttpClient client;
        private HttpClientOptions options;

        /// <summary>
        /// Instantiates a new instance of FileClient
        /// </summary>
        /// <param name="options">Options to be used for making the requests</param>
        public FileClient(HttpClientOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

            this.client = HttpClientFactory.CreateHttpClientFrom(options);
        }

        /// <summary>
        /// Performs an Http POST request
        /// </summary>
        /// <typeparam name="T">The type of the request body</typeparam>
        /// <typeparam name="R">The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="file">The body of the request</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        public async Task<HttpClientResponse<R>> HttpPost<T, R>(string route, FileInfo file, IEnumerable<KeyValuePair<string, string>> additionalContent = null, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (!file.Exists)
                throw new FileNotFoundException($"File {file.FullName} not found.");

            var request = new HttpRequestMessage(HttpMethod.Post, route);

            using (var form = new MultipartFormDataContent())
            {
                using (var content = new ByteArrayContent(File.ReadAllBytes(file.FullName)))
                {
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                    form.Add(content, "file", file.Name);

                    foreach (var item in additionalContent)
                        form.Add(new StringContent(item.Value), item.Key);

                    request.Content = content;
                }
            }

            return await this.options.RetryPolicy.Execute(async () => await this.Send<R>(request, parameters, headers));
        }

        private async Task<HttpClientResponse<T>> Send<T>(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> parameters, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            request.RequestUri = new Uri($"{request.RequestUri}/{parameters?.ToQueryString()}".Trim('/'), UriKind.Relative);

            await this.SetHeaders(request, headers);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new HttpClientResponse<T>(response, default(T));

                throw new RestClientException(response);
            }

            try
            {
                return new HttpClientResponse<T>(response, JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()));
            }
            catch (Exception e)
            {
                throw new RestClientException(e, response);
            }
        }

        private async Task SetHeaders(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (this.options.TokenProvider != null)
            {
                var token = await this.options.TokenProvider.GetToken();

                request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                    request.Headers.Add(header.Key, header.Value);
            }
        }
    }
}