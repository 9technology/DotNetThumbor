namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestBlur
    {
        [Test]
        [TestCase(1, 2)]
        [TestCase(23, 42)]
        public void ThumborBlur(int blurRadius, int? blurSigma)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Blur(blurRadius, blurSigma)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:blur({0},{1})/http://localhost/image.jpg", blurRadius, blurSigma));
        }

        [Test]
        [TestCase(0)]
        [TestCase(100)]
        public void ThumborBlurWithoutSigma(int blurRadius)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Blur(blurRadius, null)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:blur({0})/http://localhost/image.jpg", blurRadius));
        }
    }
}
