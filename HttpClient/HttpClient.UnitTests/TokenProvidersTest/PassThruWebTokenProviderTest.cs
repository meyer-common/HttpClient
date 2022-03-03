using Meyer.Common.HttpClient.TokenProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.UnitTests.TokenProvidersTest;

[TestClass]
public class PassThruWebTokenProviderTest
{
    [TestMethod]
    public async Task PassThruWebTokenProvider_CanGetToken()
    {
        var headerDictionary = new Mock<IHeaderDictionary>();
        headerDictionary.Setup(x => x.ContainsKey("Authorization")).Returns(true);
        headerDictionary.Setup(x => x["Authorization"]).Returns(new StringValues("dassdfasdf"));

        var httpRequest = new Mock<HttpRequest>();
        httpRequest.SetupGet(x => x.Headers).Returns(headerDictionary.Object);

        var httpContext = new Mock<HttpContext>();
        httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

        var tokenProvider = new PassThruWebTokenProvider(httpContextAccessor.Object);

        var tokenResponse = await tokenProvider.GetToken();

        Assert.AreEqual("dassdfasdf", tokenResponse.AccessToken);
    }

    [TestMethod]
    public async Task PassThruWebTokenProvider_NoHeader_ThrowsException()
    {
        var headerDictionary = new Mock<IHeaderDictionary>();
        headerDictionary.Setup(x => x.ContainsKey("Authorization")).Returns(false);

        var httpRequest = new Mock<HttpRequest>();
        httpRequest.SetupGet(x => x.Headers).Returns(headerDictionary.Object);

        var httpContext = new Mock<HttpContext>();
        httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

        var tokenProvider = new PassThruWebTokenProvider(httpContextAccessor.Object);

        await Assert.ThrowsExceptionAsync<TokenProviderException>(() => tokenProvider.GetToken());
    }
}