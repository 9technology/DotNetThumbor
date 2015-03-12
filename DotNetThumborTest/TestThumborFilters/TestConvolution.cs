namespace DotNetThumborTest.TestThumborFilters
{
    using System.Collections.Generic;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestConvolution
    {
        [Test]
        public void ThumborConvolution()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg").Convolution(new List<int> { 1, 2, 1 }, 3, false).ToFullUrl();

            resizedUrl.Should().Be("http://localhost/unsafe/filters:convolution(1;2;1,3,false)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborConvolutionEdgeDetection()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg").Convolution(new List<int> { -1, -1, -1, -1, 8, -1, -1, -1, -1 }, 3, false).ToFullUrl();

            resizedUrl.Should().Be("http://localhost/unsafe/filters:convolution(-1;-1;-1;-1;8;-1;-1;-1;-1,3,false)/http://localhost/image.jpg");
        }
    }
}
