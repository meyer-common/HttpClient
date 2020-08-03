using System.Net.Http;

namespace Meyer.Common.HttpClient.Compression
{
    /// <summary>
    /// Outlines methods for implementing compression for http requests
    /// </summary>
    public interface ICompression
    {
        /// <summary>
        /// Compresses the data as a ByteArrayContent
        /// </summary>
        /// <param name="data">The data to compress</param>
        /// <returns>Returns the compressed data as ByteArrayContent</returns>
        ByteArrayContent Compress(string data);
    }
}