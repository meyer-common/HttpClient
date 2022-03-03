using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient;

public class Logger
{
    private readonly ILogger<RestClient> logger;

    public Logger(ILogger<RestClient> logger)
    {
        this.logger = logger;
    }

    public async Task LogRequest(LogLevel logLevel, HttpRequestMessage request)
    {
        if (logLevel == LogLevel.None || !logger.IsEnabled(logLevel))
            return;

        var data = new
        {
            method = request.Method.ToString(),
            path = request.RequestUri.ToString(),
            headers = request.Headers,
            body = request.Content != null
                ? await request.Content.ReadAsStringAsync()
                : null
        };

        var serialized = JsonSerializer.Serialize(data);

        logger.Log(logLevel, "[{method}] {path} {NewLine} {serialized}", data.method, data.path, Environment.NewLine,
            serialized);
    }

    public async Task LogResponse(LogLevel logLevel, HttpResponseMessage response)
    {
        if (logLevel == LogLevel.None || !logger.IsEnabled(logLevel))
            return;

        var data = new
        {
            method = response.RequestMessage.Method.ToString(),
            path = response.RequestMessage.RequestUri.ToString(),
            statusCode = response.StatusCode,
            success = response.IsSuccessStatusCode,
            headers = response.Headers,
            body = response.Content != null
                ? await response.Content.ReadAsStringAsync()
                : null
        };

        var serialized = JsonSerializer.Serialize(data);

        logger.Log(logLevel, "[{method}] {path}{NewLine}{serialized}", data.method, data.path, Environment.NewLine,
            serialized);
    }
}