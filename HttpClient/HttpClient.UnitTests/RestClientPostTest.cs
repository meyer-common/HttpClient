﻿using Meyer.Common.HttpClient.TokenProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.UnitTests;

[TestClass]
public class RestClientPostTest
{
    [TestMethod]
    public async Task RestClient_CanPostAsync()
    {
        var messageHandler = new Mock<HttpMessageHandler>();

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""id"":1, ""value"":""1""}")
        };

        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new System.Net.Http.HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };

        var restClient = new RestClient(httpClient, new HttpClientOptions());

        var response = await restClient.HttpPost<dynamic, dynamic>("route", new
        {
            aaa = "aaa"
        });

        Assert.AreSame(responseMessage, response.HttpResponseMessage);

        messageHandler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.Is<HttpRequestMessage>(x => x.Content.ReadAsStringAsync().Result == @"{""aaa"":""aaa""}"),
            ItExpr.IsAny<CancellationToken>());
    }

    [TestMethod]
    public async Task RestClient_CanPost_WithNoResponse()
    {
        var messageHandler = new Mock<HttpMessageHandler>();

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(string.Empty)
        };

        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new System.Net.Http.HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };

        var restClient = new RestClient(httpClient, new HttpClientOptions());

        var response = await restClient.HttpPost<dynamic, dynamic>("route", new
        {
            aaa = "aaa"
        });

        Assert.AreSame(responseMessage, response.HttpResponseMessage);

        messageHandler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.Is<HttpRequestMessage>(x => x.Content.ReadAsStringAsync().Result == @"{""aaa"":""aaa""}"),
            ItExpr.IsAny<CancellationToken>());
    }

    [TestMethod]
    public async Task RestClient_CanPost_WithHeaders()
    {
        var messageHandler = new Mock<HttpMessageHandler>();

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""id"":1, ""value"":""1""}")
        };

        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new System.Net.Http.HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };

        var restClient = new RestClient(httpClient, new HttpClientOptions());

        var response = await restClient.HttpPost<dynamic, dynamic>("route", new
        {
            aaa = "aaa"
        },
        headers: new Dictionary<string, string>
        {
            ["aaa"] = "aaa"
        });

        Assert.AreSame(responseMessage, response.HttpResponseMessage);

        messageHandler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.Is<HttpRequestMessage>(x => x.Headers.Contains("aaa")), ItExpr.IsAny<CancellationToken>());
    }

    [TestMethod]
    public async Task RestClient_CanPost_WithParameters()
    {
        var messageHandler = new Mock<HttpMessageHandler>();

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""id"":1, ""value"":""1""}")
        };

        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new System.Net.Http.HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };

        var restClient = new RestClient(httpClient, new HttpClientOptions());

        var response = await restClient.HttpPost<dynamic, dynamic>("route", new
        {
            aaa = "aaa"
        },
        new Dictionary<string, string>
        {
            ["aaa"] = "aaa"
        });

        Assert.AreSame(responseMessage, response.HttpResponseMessage);

        messageHandler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.PathAndQuery.EndsWith("route?aaa=aaa")),
            ItExpr.IsAny<CancellationToken>());
    }

    [TestMethod]
    public async Task RestClient_CanPost_NotFound()
    {
        var messageHandler = new Mock<HttpMessageHandler>();

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
            Content = new StringContent(string.Empty)
        };

        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new System.Net.Http.HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };

        var restClient = new RestClient(httpClient, new HttpClientOptions());

        await Assert.ThrowsExceptionAsync<HttpClientException>(() => restClient.HttpPost<dynamic, dynamic>("route", new
        {
            aaa = "aaa"
        }));
    }

    [TestMethod]
    public async Task RestClient_CanPost_Exception()
    {
        var messageHandler = new Mock<HttpMessageHandler>();

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.GatewayTimeout,
            Content = new StringContent(string.Empty)
        };

        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new System.Net.Http.HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };

        var restClient = new RestClient(httpClient, new HttpClientOptions());

        await Assert.ThrowsExceptionAsync<HttpClientException>(() => restClient.HttpPost<dynamic, dynamic>("route", new
        {
            aaa = "aaa"
        }));
    }

    [TestMethod]
    public async Task RestClient_CanPost_WithTokenProvider()
    {
        var messageHandler = new Mock<HttpMessageHandler>();

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""id"":1, ""value"":""1""}")
        };

        messageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new System.Net.Http.HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };

        var token = new TokenResponse
        {
            AccessToken = "aaaa",
            Scheme = "Bearer"
        };

        var tokenProvider = new Mock<ITokenProvider>();
        tokenProvider.Setup(x => x.GetToken()).ReturnsAsync(token);

        var restClient = new RestClient(httpClient, new HttpClientOptions
        {
            TokenProvider = tokenProvider.Object
        });

        var response = await restClient.HttpPost<dynamic, dynamic>("route", new
        {
            aaa = "aaa"
        });

        Assert.AreSame(responseMessage, response.HttpResponseMessage);

        tokenProvider.Verify(x => x.GetToken(), Times.Once());

        messageHandler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.Is<HttpRequestMessage>(x =>
                x.Headers.Authorization.Scheme == token.Scheme &&
                x.Headers.Authorization.Parameter == token.AccessToken), ItExpr.IsAny<CancellationToken>());
    }
}