namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestNoUpscale
    {
        [Test]
        public void ThumborNoUpscale()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .NoUpscale(true)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:no_upscale()/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborNotNoUpscale()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .NoUpscale(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborNoUpscaleFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .NoUpscale(true)
                                    .NoUpscale(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }
    }
}
