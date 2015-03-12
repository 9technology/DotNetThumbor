namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestFill
    {
        [Test]
        [TestCase("blue")]
        [TestCase("auto")]
        [TestCase("FF0000")]
        public void ThumborFilling(string fillColour)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Fill(fillColour)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:fill({0})/http://localhost/image.jpg", fillColour));
        }

        [Test]
        public void ThumborFillingFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Fill("blue")
                                    .Fill("red")
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:fill(red)/http://localhost/image.jpg");
        }
    }
}
