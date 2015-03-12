namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestSaturation
    {
        [Test]
        [TestCase(0)]
        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.7)]
        [TestCase(2.0)]
        public void ThumborSaturationFilter(double imageSaturation)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Saturation(imageSaturation)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:saturation({0})/http://localhost/image.jpg", imageSaturation));
        }
    }
}
