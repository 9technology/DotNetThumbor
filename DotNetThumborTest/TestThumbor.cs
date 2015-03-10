namespace DotNetThumborTest
{
    using System;

    using DotNetThumbor;

    using NUnit.Framework;

    [TestFixture]
    public class TestThumbor
    {
        [Test]
        [ExpectedException(typeof(UriFormatException))]
        [TestCase("")]
        [TestCase("notaurl")]
        [TestCase("httpnoturl")]
        public void TestThumborUrlIsARealUrl(string url)
        {
            new Thumbor(url);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        [TestCase("")]
        [TestCase("notaurl")]
        [TestCase("httpnoturl")]
        public void BuildImageWithInvalidUrl(string url)
        {
            var thumbor = new Thumbor("http://localhost/");
            thumbor.BuildImage(url);
        }
    }
}
