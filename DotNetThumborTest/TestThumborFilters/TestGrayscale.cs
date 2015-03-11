namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestGrayscale
    {
        [Test]
        public void ThumborGrayscale()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Grayscale(true)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:grayscale()/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborNotGrayscale()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Grayscale(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborGrayscaleFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Grayscale(true)
                                    .Grayscale(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }
    }
}
