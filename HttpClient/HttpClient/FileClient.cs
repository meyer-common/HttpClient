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
    /// <summary>
    /// Represents an implementation of a client for performing http requests against upload endpoints
    /// </summary>
    public class FileUploadClient : IFileUploadClient
    {
        private System.Net.Http.HttpClient client;
        private HttpClientOptions options;

        /// <summary>
        /// Instantiates a new instance of FileUploadClient
        /// </summary>
        /// <param name="baseEndpoint">Sets the base absolute url</param>
        /// <param name="options">Options to be used for making the requests</param>
        public FileUploadClient(string baseEndpoint, HttpClientOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

            this.client = HttpClientFactory.CreateHttpClientFrom(baseEndpoint, options);
        }

        /// <summary>
        /// Uploads a file to an http endpoint
        /// </summary>
        /// <typeparam name="R"> The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="file">The file to upload</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        public async Task<HttpClientResponse<R>> Upload<R>(string route, FileInfo file, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            using (MultipartFormDataContent form = new MultipartFormDataContent())
            {
                StreamContent streamContent;
                using (var fileStream = file.OpenRead())
                {
                    streamContent = new StreamContent(fileStream);

                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", string.Format("form-data; name=\"file\"; filename=\"{0}\"", file.Name));
                    form.Add(streamContent, "file", file.Name);

                    return await this.options.RetryPolicy.Execute(async () => await this.Send<R>(new HttpRequestMessage(HttpMethod.Post, route) { Content = form }, parameters, headers));
                }
            }
        }

        private async Task<HttpClientResponse<T>> Send<T>(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> parameters, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            request.RequestUri = new Uri($"{request.RequestUri}{parameters?.ToQueryString()}", UriKind.Relative);

            await this.SetHeaders(request, headers);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new HttpClientResponse<T>(response, default(T));

                throw new HttpClientException(response);
            }

            try
            {
                return new HttpClientResponse<T>(response, JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()));
            }
            catch (Exception e)
            {
                throw new HttpClientException(e, response);
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