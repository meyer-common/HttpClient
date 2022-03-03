using Meyer.Common.HttpClient.Policies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Meyer.Common.HttpClient.Tests.Unit;

[TestClass]
public class HttpClientOptionsTest
{
    [TestMethod]
    public void HttpClientOptions_RetryPolicy_DefaultsToNone()
    {
        var options = new HttpClientOptions();

        Assert.IsInstanceOfType(options.RetryPolicy, typeof(NoRetryPolicy));
    }

    [TestMethod]
    public void HttpClientOptions_SetNullRetryPolicy_ThrowsException()
    {
        var options = new HttpClientOptions();

        Assert.ThrowsException<ArgumentNullException>(() => options.RetryPolicy = null);
    }

    [TestMethod]
    public void HttpClientOptions_Timeout_DefaultsTo60()
    {
        var options = new HttpClientOptions();

        Assert.AreEqual(60, options.Timeout.TotalSeconds);
    }
}