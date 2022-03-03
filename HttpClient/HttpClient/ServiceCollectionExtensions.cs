using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;

namespace Meyer.Common.HttpClient;

public static class ServiceCollectionExtensions
{
    public static void AddRestClient<TService, TImplementation, TOptions>(this IServiceCollection services,
        string baseUrl, Action<TOptions> configureClient)
        where TService : class, IRestClient
        where TImplementation : class, TService
        where TOptions : HttpClientOptions, new()
    {
        var options = new TOptions();
        configureClient(options);

        services.AddSingleton<Logger>();

        services.AddSingleton(options);

        services
            .AddHttpClient<TService>(x =>
            {
                x.BaseAddress = new Uri($"{baseUrl.Trim('/')}/");
                x.Timeout = options.Timeout;
            })
            .ConfigurePrimaryHttpMessageHandler(x =>
            {
                var handler = new HttpClientHandler();

                if (options.Proxy != null && handler.SupportsProxy)
                    handler.Proxy = options.Proxy;

                if (options.SupportsAutomaticDecompression && handler.SupportsAutomaticDecompression)
                    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                return handler;
            });

        services.AddTransient<TService, TImplementation>();
    }
}