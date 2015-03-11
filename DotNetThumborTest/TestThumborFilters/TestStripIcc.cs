namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestStripIcc
    {
        [Test]
        public void ThumborStripIcc()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .StripIcc(true)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:strip_icc()/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborNotStripIcc()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .StripIcc(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborStripIccFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .StripIcc(true)
                                    .StripIcc(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }
    }
}
