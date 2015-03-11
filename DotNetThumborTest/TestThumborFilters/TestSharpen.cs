namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestSharpen
    {
        [Test]
        [TestCase(1.0, 2.0, true)]
        [TestCase(0.6, 0.2, true)]
        [TestCase(1.1, 1.7, false)]
        public void ThumborSharpenFilter(double amount, double radius, bool luminance)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Sharpen(amount, radius, luminance)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:sharpen({0},{1},{2})/http://localhost/image.jpg", amount, radius, luminance.ToString().ToLower()));
        }
    }
}
