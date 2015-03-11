namespace DotNetThumborTest.TestThumborFilters
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestRoundCorners
    {
        [Test]
        public void ThumborRoundCornersFilter()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .RoundCorners(10, 20, 1, 2, 3)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:round_corners(10|20,1,2,3)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborRoundCornersFilterWithRadiusB()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .RoundCorners(10, null, 100, 50, 0)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:round_corners(10,100,50,0)/http://localhost/image.jpg");
        }
    }
}
