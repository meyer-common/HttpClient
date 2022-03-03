using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;

namespace Meyer.Common.HttpClient;

/// <summary>
/// Extension methods for adding a rest client with its dependencies
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a restclient with its dependencies to the service collection
    /// </summary>
    /// <typeparam name="TService">A typing interface of type IRestClient</typeparam>
    /// <typeparam name="TImplementation">A typing implementation of IRestClient</typeparam>
    /// <typeparam name="TOptions">A typing implementation of HttpClientOptions</typeparam>
    /// <param name="services">A service collection</param>
    /// <param name="baseAddress">The base address</param>
    /// <param name="configureClient">An Action to configure HttpClientOptions</param>
    public static void AddRestClient<TService, TImplementation, TOptions>(this IServiceCollection services, string baseAddress, Action<TOptions> configureClient)
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
                x.BaseAddress = new Uri($"{baseAddress.Trim('/')}/");
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