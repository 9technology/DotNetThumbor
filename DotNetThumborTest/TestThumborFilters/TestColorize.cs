namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestColorize
    {
        [Test]
        public void ThumborColorizeFilter()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Colorize(1, 2, 3, "AAAAAA")
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:colorize(1,2,3,AAAAAA)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborClearColorizeFilter()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Colorize(1, 2, 3, "AAAAAA")
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:colorize(1,2,3,AAAAAA)/http://localhost/image.jpg");
        }
    }
}
