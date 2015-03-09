namespace DotNetThumborTest
{
    using System;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestThumbor
    {
        [Test]
        [ExpectedException(typeof(UriFormatException))]
        [TestCase("")]
        [TestCase("notaurl")]
        [TestCase("httpnoturl")]
        public void TestThumborUrlIsARealUrl(string url)
        {
            new Thumbor(url);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToUrlThrowExceptionIfBuildImageNotCalled()
        {
            var thumbor = new Thumbor("http://localhost/");
            thumbor.ToUrl();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        [TestCase("")]
        [TestCase("notaurl")]
        [TestCase("httpnoturl")]
        public void BuildImageWithInvalidUrl(string url)
        {
            var thumbor = new Thumbor("http://localhost/");
            thumbor.BuildImage(url);
        }

        [Test]
        public void ThumborBuildUrlToUrlReturnsCorrectUrl()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNoResizing()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, null)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNoResizingWithZero()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(0, 0)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthOnly()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(10, null)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/10x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeHeightOnly()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, 10)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResize()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(20, 30)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/20x30/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthNegativeFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(-10, null)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-10x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeHeightNegativeFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, -10)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x-10/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborSmart()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/smart/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNotSmart()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(false)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborSmartIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(false)
                                    .Smart(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/smart/http://localhost/image.jpg");
        }

        [Test]
        [TestCase(Thumbor.ImageFormat.Jpeg)]
        [TestCase(Thumbor.ImageFormat.Png)]
        [TestCase(Thumbor.ImageFormat.Gif)]
        [TestCase(Thumbor.ImageFormat.Webp)]
        public void ThumborFormat(Thumbor.ImageFormat format)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Format(format)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:format({0})/http://localhost/image.jpg", format.ToString().ToLower()));
        }

        [Test]
        public void ThumborFormatIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Format(Thumbor.ImageFormat.Png)
                                    .Format(Thumbor.ImageFormat.Gif)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:format(gif)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborCrop()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Crop(0, 10, 50, 100)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10:50x100/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborCropIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Crop(100, 200, 300, 400)
                                    .Crop(0, 10, 50, 100)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10:50x100/http://localhost/image.jpg");
        }

        [Test]
        [TestCase(-100)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(99)]
        [TestCase(100)]
        [TestCase(1000)]
        public void ThumborQuality(int quality)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Quality(quality)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:quality({0})/http://localhost/image.jpg", quality.ToString()));
        }

        [Test]
        public void ThumborQualitySetTwiceFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Quality(10)
                                    .Quality(99)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:quality(99)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborGrayscale()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Grayscale(true)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:grayscale()/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborNotGrayscale()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Grayscale(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborGrayscaleFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Grayscale(true)
                                    .Grayscale(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        [TestCase("http://image.url", 1, 2, 3)]
        [TestCase("http://localhost/image.jpg", 99, 23, 42)]
        public void ThumborWatermark(string imageUrl, int right, int down, int transparency)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Watermark(imageUrl, right, down, transparency)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:watermark({0},{1},{2},{3})/http://localhost/image.jpg", imageUrl, right, down, transparency));
        }

        [Test]
        public void ThumborWatermarks()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Watermark("http://image1.url", 1, 3, 5)
                                    .Watermark("http://image2.url", 2, 4, 6)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:watermark(http://image1.url,1,3,5):watermark(http://image2.url,2,4,6)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborWatermarkUsingThumbor()
        {
            var watermark = new Thumbor("http://localhost/");

            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Watermark(watermark.BuildImage("http://localhost/watermark.png"), 0, 0, 50)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:watermark(http://localhost/unsafe/http://localhost/watermark.png,0,0,50)/http://localhost/image.jpg");
        }

        [Test]
        [TestCase("blue")]
        [TestCase("auto")]
        [TestCase("FF0000")]
        public void ThumborFilling(string fillColour)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Fill(fillColour)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/filters:fill({0})/http://localhost/image.jpg", fillColour));
        }

        [Test]
        public void ThumborFillingFirstIgnored()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Fill("blue")
                                    .Fill("red")
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:fill(red)/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborTrim()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(true)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/trim/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborNotTrim()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborTrimIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(true)
                                    .Trim(false)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborMultiTest()
        {
            var watermark = new Thumbor("http://localhost");
            watermark.BuildImage(
                "https://localhost/watermark.png");

            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(true)
                                    .Resize(200, 400)
                                    .Grayscale(false)
                                    .Fill("blue")
                                    .Quality(100)
                                    .Watermark(watermark, 0, 10, 50)
                                    .Smart(true)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/trim/200x400/smart/filters:quality(100):watermark(http://localhost/unsafe/https://localhost/watermark.png,0,10,50):fill(blue)/http://localhost/image.jpg"));
        }
    }
}
