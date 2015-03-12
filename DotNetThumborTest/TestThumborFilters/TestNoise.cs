namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestNoise
    {
        [Test]
        [TestCase(2)]
        [TestCase(20)]
        public void ThumborNoiseFilter(int noise)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Noise(noise)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:noise({0})/http://localhost/image.jpg", noise));
        }
    }
}
