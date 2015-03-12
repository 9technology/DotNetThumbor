namespace DotNetThumborTest
{
    using System;

    using DotNetThumbor;

    using FluentAssertions;

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

        [Test]
        [TestCase("/trim/100x200/filters:grayscale()/http://myserver/myimage.jpg")]
        public void BuildUrlWithoutSecretKey(string url)
        {
            var thumbor = new Thumbor("http://localhost/");
            var thumborUrl = thumbor.BuildUrl(url);
            thumborUrl.Should().Be("http://localhost/unsafe/trim/100x200/filters:grayscale()/http://myserver/myimage.jpg");
        }

        [Test]
        [TestCase("/trim/100x200/filters:grayscale()/http://myserver/myimage.jpg")]
        public void BuildUrlWithSecretKey(string url)
        {
            var thumbor = new Thumbor("http://localhost/", "secret_key");
            var thumborUrl = thumbor.BuildUrl(url);
            thumborUrl.Should().Be("http://localhost/3O6tySGiWNKehjeS1ARFGEnNMtU=/trim/100x200/filters:grayscale()/http://myserver/myimage.jpg");
        }
    }
}
