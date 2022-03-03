using Meyer.Common.HttpClient.TokenProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Meyer.Common.HttpClient.Tests.Unit.TokenProvidersTest;

[TestClass]
public class StaticTokenProviderTest
{
    [TestMethod]
    public async Task TokenResponse_CanSetToken()
    {
        var tokenProvider = new StaticTokenProvider();

        var token = new TokenResponse
        {
            AccessToken = "adasdasd",
            ExpiresIn = 40,
            Scheme = "dsfafasdf"
        };

        tokenProvider.SetToken(token);

        Assert.AreSame(token, await tokenProvider.GetToken());
    }
}