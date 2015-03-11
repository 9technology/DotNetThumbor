namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestGifV
    {
        [Test]
        public void ThumborGifV()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .GifV(Thumbor.ImageGifVOption.None)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:gifv()/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborGifVConvertToWebm()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .GifV(Thumbor.ImageGifVOption.Webm)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:gifv(webm)/http://localhost/image.jpg"));
        }
    }
}
