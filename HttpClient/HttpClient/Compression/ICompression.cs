using System.Net.Http;

namespace Meyer.Common.HttpClient.Compression
{
    public interface ICompression
    {
        ByteArrayContent Compress(string data);
    }
}