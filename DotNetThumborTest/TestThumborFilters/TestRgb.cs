namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestRgb
    {
        [Test]
        [TestCase(1, 2, 3)]
        [TestCase(100, 50, 0)]
        public void ThumborRgbFilter(int red, int green, int blue)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Rgb(red, green, blue)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:rgb({0},{1},{2})/http://localhost/image.jpg", red, green, blue));
        }
    }
}
