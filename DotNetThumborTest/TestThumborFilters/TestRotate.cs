namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestRotate
    {
        [Test]
        [TestCase(0)]
        [TestCase(90)]
        [TestCase(180)]
        [TestCase(270)]
        public void ThumborRotateFilter(int imageRotateAngle)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Rotate(imageRotateAngle)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:rotate({0})/http://localhost/image.jpg", imageRotateAngle));
        }
    }
}
