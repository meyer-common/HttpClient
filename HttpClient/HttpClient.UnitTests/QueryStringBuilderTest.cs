using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Meyer.Common.HttpClient.UnitTests;

[TestClass]
public class QueryStringBuilderTest
{
    [TestMethod]
    public void QueryStringBuilder_SingleElement()
    {
        var queryString = new Dictionary<string, string>
        {
            ["element1"] = "aaa"
        }.ToQueryString();

        Assert.AreEqual("?element1=aaa", queryString);
    }

    [TestMethod]
    public void QueryStringBuilder_MultiElements()
    {
        var queryString = new Dictionary<string, string>
        {
            ["element1"] = "aaa",
            ["element2"] = "bbb"
        }.ToQueryString();

        Assert.AreEqual("?element1=aaa&element2=bbb", queryString);
    }

    [TestMethod]
    public void QueryStringBuilder_NoElements()
    {
        var queryString = (null as Dictionary<string, string>).ToQueryString();

        Assert.AreEqual(string.Empty, queryString);
    }

    [TestMethod]
    public void QueryStringBuilder_EscapesElements()
    {
        var queryString = new Dictionary<string, string>
        {
            ["element1"] = "aaa",
            ["element2"] = "bb=b"
        }.ToQueryString();

        Assert.AreEqual("?element1=aaa&element2=bb%3Db", queryString);
    }
}