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
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToUrlThrowExceptionIfBuildImageNotCalled()
        {
            var thumbor = new Thumbor("http://localhost/");
            thumbor.ToUrl();
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
        public void ThumborBuildUrlToUrlReturnsCorrectUrl()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNoResizing()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, null)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNoResizingWithZero()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(0, 0)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthOnly()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(10, null)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/10x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeHeightOnly()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, 10)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResize()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(20, 30)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/20x30/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborSmart()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/smart/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNotSmart()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(false)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        [TestCase(Thumbor.ImageFormat.Jpeg)]
        [TestCase(Thumbor.ImageFormat.Png)]
        [TestCase(Thumbor.ImageFormat.Gif)]
        [TestCase(Thumbor.ImageFormat.Webp)]
        public void ThumborFormat(Thumbor.ImageFormat format)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Format(format)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:format({0})/http://localhost/image.jpg", format.ToString().ToLower()));
        }

        [Test]
        public void ThumborCrop()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Crop(0, 10, 50, 100)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10:50x100/http://localhost/image.jpg");
        }
    }
}
