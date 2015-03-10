namespace DotNetThumborTest
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestThumborSigner
    {

        [Test]
        public void TestMethod1()
        {
            var key = Encoding.UTF8.GetBytes("");
            var t = new ThumborSigner();

            var list = new List<string>();

            var input = "https://searchcode.com/static/searchcode_logo.png";

            list.Add(input);
            
            var result = t.Encode(string.Join("/", list), key);

            var url = string.Format("http://test/{0}/{1}", result, input);

        }
    }
}
