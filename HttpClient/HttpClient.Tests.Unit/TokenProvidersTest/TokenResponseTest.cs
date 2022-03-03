using Meyer.Common.HttpClient.TokenProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Meyer.Common.HttpClient.Tests.Unit.TokenProvidersTest;

[TestClass]
public class TokenResponseTest
{
    [TestMethod]
    public void Token_IsNotExpired()
    {
        var token = new TokenResponse
        {
            ExpiresIn = 3600
        };

        Assert.IsFalse(token.IsExpired());
    }

    [TestMethod]
    public void Token_IsExpired_IfLessThan90()
    {
        var token = new TokenResponse
        {
            ExpiresIn = 40
        };

        Assert.IsTrue(token.IsExpired());
    }

    [TestMethod]
    public void Token_IsExpired_If0()
    {
        var token = new TokenResponse
        {
            ExpiresIn = 0
        };

        Assert.IsTrue(token.IsExpired());
    }
}