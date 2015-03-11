namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestFormat
    {
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
        public void ThumborFormatIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Format(Thumbor.ImageFormat.Png)
                                    .Format(Thumbor.ImageFormat.Gif)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:format(gif)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborFormatNone()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Format(Thumbor.ImageFormat.None)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }
    }
}
