using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;

namespace Meyer.Common.HttpClient.Compression
{
    /// <summary>
    /// Implements Gzip compression
    /// </summary>
    public class GzipCompression : ICompression
    {
        /// <summary>
        /// Compresses the data as a ByteArrayContent
        /// </summary>
        /// <param name="data">The data to compress</param>
        /// <returns>Returns the compressed data as ByteArrayContent</returns>
        public ByteArrayContent Compress(string data)
        {
            using (var dataStream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                using (var compressed = new MemoryStream())
                {
                    using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
                    {
                        gzip.Write(dataStream.ToArray(), 0, (int)dataStream.Length);
                    }

                    return new ByteArrayContent(compressed.ToArray());
                }
            }
        }
    }
}