using System.Net.Http;

namespace Meyer.Common.HttpClient
{
    /// <summary>
    /// Represents a response from the http request
    /// </summary>
    /// <typeparam name="T">The System.Type used to deserialize the response body</typeparam>
    public class RestClientResponse<T>
    {
        /// <summary>
        /// Gets the full http response details
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; }

        /// <summary>
        /// Gets the deserialized response body
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Instantiates a new instance of RestClientResponse
        /// </summary>
        /// <param name="httpResponseMessage">The full http response details</param>
        /// <param name="result">The deserialized response body</param>
        public RestClientResponse(HttpResponseMessage httpResponseMessage, T result)
        {
            this.HttpResponseMessage = httpResponseMessage;
            this.Result = result;
        }
    }
}