using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient
{
    /// <summary>
    /// Interface outlines methods for performing upload requests with files
    /// </summary>
    public interface IFileUploadClient
    {
        /// <summary>
        /// Uploads a file to an http endpoint
        /// </summary>
        /// <typeparam name="R"> The type of the response body</typeparam>
        /// <param name="route">The route to add to the base address (ex: entites/1234)</param>
        /// <param name="file">The file to upload</param>
        /// <param name="parameters">Optional query parameters to add to the request</param>
        /// <param name="headers">Optional headers to add to the request</param>
        /// <returns>Returns the parsed body as type R</returns>
        Task<HttpClientResponse<R>> Upload<R>(string route, FileInfo file, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null);
    }
}