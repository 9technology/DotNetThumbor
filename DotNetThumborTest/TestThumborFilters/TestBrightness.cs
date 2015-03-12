namespace DotNetThumborTest.TestThumborFilters
{
    using System.Globalization;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestBrightness
    {
        [Test]
        [TestCase(-100)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(99)]
        [TestCase(100)]
        [TestCase(1000)]
        public void ThumborBrightnessFilter(int brightness)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Brightness(brightness)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:brightness({0})/http://localhost/image.jpg", brightness.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void ThumborBrightnessFilterFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Brightness(10)
                                    .Brightness(99)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:brightness(99)/http://localhost/image.jpg");
        }
    }
}
