namespace DotNetThumborTest.TestThumborFilters
{
    using System.Globalization;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestQuality
    {
        [Test]
        [TestCase(-100)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(99)]
        [TestCase(100)]
        [TestCase(1000)]
        public void ThumborQuality(int quality)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Quality(quality)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:quality({0})/http://localhost/image.jpg", quality.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void ThumborQualitySetTwiceFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Quality(10)
                                    .Quality(99)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:quality(99)/http://localhost/image.jpg");
        }
    }
}
