namespace DotNetThumborTest
{
    using System;

    using DotNetThumbor;

    using NUnit.Framework;

    using FluentAssertions;

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
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToUrlThrowExceptionIfBuildImageNotCalled()
        {
            var thumbor = new Thumbor("http://localhost/");
            thumbor.ToUrl();
        }

        [Test]
        public void ThumborBuildUrlToUrlReturnsCorrectUrl()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }
    }
}
