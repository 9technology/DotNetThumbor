namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestMaxBytes
    {
        [Test]
        [TestCase(2)]
        [TestCase(20)]
        public void ThumborMaxBytesFilter(int maxBytes)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .MaxBytes(maxBytes)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:max_bytes({0})/http://localhost/image.jpg", maxBytes));
        }
    }
}
