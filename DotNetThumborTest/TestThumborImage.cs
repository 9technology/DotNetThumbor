namespace DotNetThumborTest
{
    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestThumborImage
    {
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
        public void ThumborResizeWidthNegativeZeroFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(-0, -0)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthZeroFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(-0, -0)
                                    .HorizontalFlip(true)
                                    .VerticalFlip(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-0x-0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborFlipWithNoResize()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .HorizontalFlip(true)
                                    .VerticalFlip(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-0x-0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthNegativeWidthNegativeHeightFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(-10, -20)
                                    .HorizontalFlip(true)
                                    .VerticalFlip(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/10x20/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthNegativeWidthNegativeHeightNoFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(-10, -20)
                                    .HorizontalFlip(false)
                                    .VerticalFlip(false)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-10x-20/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeZeroWidthNegativeHeightNoFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(0, -20)
                                    .HorizontalFlip(false)
                                    .VerticalFlip(false)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x-20/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizZeroWidthNegativeHeightFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(0, -20)
                                    .HorizontalFlip(true)
                                    .VerticalFlip(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-0x20/http://localhost/image.jpg");
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
            var thumbor = new Thumbor("http://localhost/");

            var watermark = thumbor.BuildImage("http://localhost/watermark.png").ToUrl();

            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Watermark(watermark, 0, 0, 50)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:watermark(http://localhost/unsafe/http://localhost/watermark.png,0,0,50)/http://localhost/image.jpg");
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
        [TestCase(true)]
        [TestCase(false)]
        public void ThumborFitIn(bool fitIn)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .FitIn(fitIn)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}http://localhost/image.jpg", fitIn ? "fit-in/" : string.Empty));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ThumborFullFitIn(bool fullFitIn)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .FullFitIn(fullFitIn)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}http://localhost/image.jpg", fullFitIn ? "full-fit-in/" : string.Empty));
        }

        [Test]
        public void ThumborFitInChangeToFullFitIn()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .FitIn(true)
                                    .FullFitIn(true)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/full-fit-in/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborFitInRemoveUsingFullFitIn()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .FitIn(true)
                                    .FullFitIn(false)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        [TestCase(Thumbor.ImageHorizontalAlign.Left)]
        [TestCase(Thumbor.ImageHorizontalAlign.Right)]
        public void ThumborHorizontalAlign(Thumbor.ImageHorizontalAlign align)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .HorizontalAlign(align)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}/http://localhost/image.jpg", align.ToString().ToLower()));
        }

        [Test]
        public void ThumborHorizontalAlignCenter()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .HorizontalAlign(Thumbor.ImageHorizontalAlign.Center)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        [TestCase(Thumbor.ImageHorizontalAlign.Left, Thumbor.ImageHorizontalAlign.Center)]
        [TestCase(Thumbor.ImageHorizontalAlign.Right, Thumbor.ImageHorizontalAlign.Left)]
        [TestCase(Thumbor.ImageHorizontalAlign.Center, Thumbor.ImageHorizontalAlign.Right)]
        public void ThumborVerticalAlignIgnoreFirst(Thumbor.ImageHorizontalAlign firstAlign, Thumbor.ImageHorizontalAlign secondAlign)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .HorizontalAlign(firstAlign)
                                    .HorizontalAlign(secondAlign)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}http://localhost/image.jpg", secondAlign == Thumbor.ImageHorizontalAlign.Center ? string.Empty : secondAlign.ToString().ToLower() + "/"));
        }

        [Test]
        [TestCase(Thumbor.ImageVerticalAlign.Top)]
        [TestCase(Thumbor.ImageVerticalAlign.Bottom)]
        public void ThumborVerticalAlign(Thumbor.ImageVerticalAlign align)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .VerticalAlign(align)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}/http://localhost/image.jpg", align.ToString().ToLower()));
        }

        [Test]
        public void ThumborVerticalAlignMiddle()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .VerticalAlign(Thumbor.ImageVerticalAlign.Middle)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        [TestCase(Thumbor.ImageVerticalAlign.Top, Thumbor.ImageVerticalAlign.Middle)]
        [TestCase(Thumbor.ImageVerticalAlign.Bottom, Thumbor.ImageVerticalAlign.Top)]
        [TestCase(Thumbor.ImageVerticalAlign.Middle, Thumbor.ImageVerticalAlign.Bottom)]
        public void ThumborVerticalAlignIgnoreFirst(Thumbor.ImageVerticalAlign firstAlign, Thumbor.ImageVerticalAlign secondAlign)
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .VerticalAlign(firstAlign)
                                    .VerticalAlign(secondAlign)
                                    .ToUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}http://localhost/image.jpg", secondAlign == Thumbor.ImageVerticalAlign.Middle ? string.Empty : secondAlign.ToString().ToLower() + "/"));
        }

        [Test]
        public void ThumborMultiTest()
        {
            var thumbor = new Thumbor("http://localhost/");

            var watermark = thumbor.BuildImage("https://localhost/watermark.png")
                                   .HorizontalFlip(true)
                                   .VerticalFlip(true)
                                   .ToUrl();

            var resizedUrl = thumbor.BuildImage("https://localhost/image.jpg")
                                    .Trim(true)
                                    .Resize(200, 400)
                                    .Grayscale(true)
                                    .Fill("blue")
                                    .Quality(100)
                                    .Watermark(watermark, 0, 10, 50)
                                    .Smart(true)
                                    .FitIn(true)
                                    .HorizontalAlign(Thumbor.ImageHorizontalAlign.Left)
                                    .VerticalAlign(Thumbor.ImageVerticalAlign.Bottom)
                                    .Format(Thumbor.ImageFormat.Webp)
                                    .ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/trim/fit-in/200x400/left/bottom/smart/filters:format(webp):quality(100):grayscale():watermark(http://localhost/unsafe/-0x-0/https://localhost/watermark.png,0,10,50):fill(blue)/https://localhost/image.jpg");
        }

        [Test]
        public void ThumborNonFluentTest()
        {
            var thumbor = new Thumbor("http://localhost/");

            var watermarkImage = thumbor.BuildImage("https://localhost/watermark.png");
            watermarkImage.HorizontalFlip(true);
            watermarkImage.VerticalFlip(true);
            var watermark = watermarkImage.ToUrl();

            var resizedUrlImage = thumbor.BuildImage("https://localhost/image.jpg");
            resizedUrlImage.Trim(true);
            resizedUrlImage.Resize(200, 400);
            resizedUrlImage.Grayscale(true);
            resizedUrlImage.Fill("blue");
            resizedUrlImage.Quality(100);
            resizedUrlImage.Watermark(watermark, 0, 10, 50);
            resizedUrlImage.Smart(true);
            resizedUrlImage.FitIn(true);
            resizedUrlImage.HorizontalAlign(Thumbor.ImageHorizontalAlign.Left);
            resizedUrlImage.VerticalAlign(Thumbor.ImageVerticalAlign.Bottom);
            resizedUrlImage.Format(Thumbor.ImageFormat.Webp);

            var resizedUrl = resizedUrlImage.ToUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/trim/fit-in/200x400/left/bottom/smart/filters:format(webp):quality(100):grayscale():watermark(http://localhost/unsafe/-0x-0/https://localhost/watermark.png,0,10,50):fill(blue)/https://localhost/image.jpg");
        }

        [Test]
        public void ThumborSignedUrl()
        {
            var thumbor = new Thumbor("http://localhost/", "sample_key");
            var resizedUrl = thumbor.BuildImage("https://localhost/image.jpg").ToUrl();

            resizedUrl.Should().Be("http://localhost/_fak0PqFdoaKkMQpbxPE0ql8dtY=/https://localhost/image.jpg");
        }
    }
}
