namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestWatermark
    {
        [Test]
        [TestCase("http://image.url", "1", "2", 3)]
        [TestCase("http://localhost/image.jpg", "99", "23", 42)]
        [TestCase("http://localhost/image.jpg", "center", "center", 42)]
        [TestCase("http://localhost/image.jpg", "repeat", "repeat", 42)]
        [TestCase("http://localhost/image.jpg", "20p", "20p", 42)]
        public void ThumborWatermark(string imageUrl, string right, string down, int transparency)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Watermark(imageUrl, right, down, transparency)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:watermark({0},{1},{2},{3})/http://localhost/image.jpg", imageUrl, right, down, transparency));
        }

        [Test]
        public void ThumborWatermarks()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Watermark("http://image1.url", "1", "3", 5)
                                    .Watermark("http://image2.url", "2", "4", 6)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:watermark(http://image1.url,1,3,5):watermark(http://image2.url,2,4,6)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborWatermarkUsingThumbor()
        {
            var thumbor = new Thumbor("http://localhost/");

            var watermark = thumbor.BuildImage("http://localhost/watermark.png").ToFullUrl();

            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Watermark(watermark, "0", "0", 50)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:watermark(http://localhost/unsafe/http://localhost/watermark.png,0,0,50)/http://localhost/image.jpg");
        }
    }
}
