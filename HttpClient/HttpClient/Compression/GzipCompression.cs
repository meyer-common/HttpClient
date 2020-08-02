using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;

namespace Meyer.Common.HttpClient.Compression
{
    public class GzipCompression : ICompression
    {
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