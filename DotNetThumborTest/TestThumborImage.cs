namespace DotNetThumborTest
{
    using System;
    using System.Collections.Generic;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestThumborImage
    {
        [Test]
        public void ThumborBuildUrlToFullUrlReturnsCorrectUrl()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborBuildUrlToUrlReturnsCorrectUrl()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .ToUrl();
            resizedUrl.Should().Be("unsafe/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNoResizing()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, null)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNoResizingWithZero()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(0, 0)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthOnly()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(10, null)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/10x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeHeightOnly()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, 10)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResize()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(20, 30)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/20x30/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthNegativeZeroFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(-0, -0)
                                    .ToFullUrl();
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
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-0x-0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborFlipWithNoResize()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .HorizontalFlip(true)
                                    .VerticalFlip(true)
                                    .ToFullUrl();
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
                                    .ToFullUrl();
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
                                    .ToFullUrl();
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
                                    .ToFullUrl();
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
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-0x20/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeWidthNegativeFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(-10, null)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/-10x0/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborResizeHeightNegativeFlip()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Resize(null, -10)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x-10/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborSmart()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(true)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/smart/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborNotSmart()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(false)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborSmartIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Smart(false)
                                    .Smart(true)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/smart/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborCrop()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Crop(0, 10, 50, 100)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10:50x100/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborCropIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Crop(100, 200, 300, 400)
                                    .Crop(0, 10, 50, 100)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/0x10:50x100/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborTrim()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(Thumbor.ImageTrimOption.None)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborTrimTopLeft()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(Thumbor.ImageTrimOption.TopLeft)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/trim/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborTrimBottomRight()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(Thumbor.ImageTrimOption.BottomRight)
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/trim:bottom-right/http://localhost/image.jpg"));
        }

        [Test]
        public void ThumborTrimIgnoresFirst()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .Trim(Thumbor.ImageTrimOption.BottomRight)
                                    .Trim(Thumbor.ImageTrimOption.None)
                                    .ToFullUrl();
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
                                    .ToFullUrl();
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
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}http://localhost/image.jpg", fullFitIn ? "full-fit-in/" : string.Empty));
        }

        [Test]
        public void ThumborFitInChangeToFullFitIn()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .FitIn(true)
                                    .FullFitIn(true)
                                    .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/full-fit-in/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborFitInRemoveUsingFullFitIn()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .FitIn(true)
                                    .FullFitIn(false)
                                    .ToFullUrl();
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
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}/http://localhost/image.jpg", align.ToString().ToLower()));
        }

        [Test]
        public void ThumborHorizontalAlignCenter()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .HorizontalAlign(Thumbor.ImageHorizontalAlign.Center)
                                    .ToFullUrl();
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
                                    .ToFullUrl();
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
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}/http://localhost/image.jpg", align.ToString().ToLower()));
        }

        [Test]
        public void ThumborVerticalAlignMiddle()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl = thumbor.BuildImage("http://localhost/image.jpg")
                                    .VerticalAlign(Thumbor.ImageVerticalAlign.Middle)
                                    .ToFullUrl();
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
                                    .ToFullUrl();
            resizedUrl.Should().Be(string.Format("http://localhost/unsafe/{0}http://localhost/image.jpg", secondAlign == Thumbor.ImageVerticalAlign.Middle ? string.Empty : secondAlign.ToString().ToLower() + "/"));
        }

        [Test]
        public void ThumborMultiTest()
        {
            var thumbor = new Thumbor("http://localhost/");

            var watermark = thumbor.BuildImage("https://localhost/watermark.png")
                                   .HorizontalFlip(true)
                                   .VerticalFlip(true)
                                   .ToFullUrl();

            var resizedUrl = thumbor.BuildImage("https://localhost/image.jpg")
                                    .Trim(Thumbor.ImageTrimOption.TopLeft)
                                    .Resize(200, 400)
                                    .Smart(true)
                                    .FitIn(true)
                                    .HorizontalAlign(Thumbor.ImageHorizontalAlign.Left)
                                    .VerticalAlign(Thumbor.ImageVerticalAlign.Bottom)
                                    .Blur(1,2)
                                    .Brightness(50)
                                    .Colorize(1,2,3,"AAAAAA")
                                    .Contrast(50)
                                    .Convolution(new List<int> { 1, 2, 1 }, 3, false)
                                    .Curve(new List<Tuple<int, int>> { Tuple.Create(1, 2) }, new List<Tuple<int, int>> { Tuple.Create(3, 4) }, new List<Tuple<int, int>> { Tuple.Create(5, 6) }, new List<Tuple<int, int>> { Tuple.Create(7, 8) })
                                    .Equalize(true)
                                    .ExtractFocal()
                                    .Fill("blue")
                                    .Format(Thumbor.ImageFormat.Webp)
                                    .GifV(Thumbor.ImageGifVOption.Webm)
                                    .Grayscale(true)
                                    .MaxBytes(100000)
                                    .Noise(50)
                                    .NoUpscale(true)
                                    .Quality(100)
                                    .Rgb(1,2,3)
                                    .Rotate(90)
                                    .RoundCorners(10, 20, 1, 2, 3)
                                    .Saturation(1.7)
                                    .Sharpen(5.0, 1.2, true)
                                    .StripIcc(true)
                                    .Watermark(watermark, "0", "10", 50)
                                    .ToFullUrl();
            // Verified as a value URL when used against internal thumbor server
            resizedUrl.Should().Be("http://localhost/unsafe/trim/fit-in/200x400/left/bottom/smart/filters:blur(1,2):brightness(50):colorize(1,2,3,AAAAAA):contrast(50):convolution(1;2;1,3,false):curve([(1,2)],[(3,4)],[(5,6)],[(7,8)]):equalize():extract_focal():fill(blue):format(webp):gifv(webm):grayscale():max_bytes(100000):noise(50):no_upscale():quality(100):rgb(1,2,3):rotate(90):round_corners(10|20,1,2,3):saturation(1.7):sharpen(5,1.2,true):strip_icc():watermark(http://localhost/unsafe/-0x-0/https://localhost/watermark.png,0,10,50)/https://localhost/image.jpg");
        }

        [Test]
        public void ThumborNonFluentTest()
        {
            var thumbor = new Thumbor("http://localhost/");

            var watermarkImage = thumbor.BuildImage("https://localhost/watermark.png");
            watermarkImage.HorizontalFlip(true);
            watermarkImage.VerticalFlip(true);
            var watermark = watermarkImage.ToFullUrl();

            var resizedUrlImage = thumbor.BuildImage("https://localhost/image.jpg");
            resizedUrlImage.Trim(Thumbor.ImageTrimOption.TopLeft);
            resizedUrlImage.Resize(200, 400);
            resizedUrlImage.Grayscale(true);
            resizedUrlImage.Fill("blue");
            resizedUrlImage.Quality(100);
            resizedUrlImage.Watermark(watermark, "0", "10", 50);
            resizedUrlImage.Smart(true);
            resizedUrlImage.FitIn(true);
            resizedUrlImage.HorizontalAlign(Thumbor.ImageHorizontalAlign.Left);
            resizedUrlImage.VerticalAlign(Thumbor.ImageVerticalAlign.Bottom);
            resizedUrlImage.Format(Thumbor.ImageFormat.Webp);

            var resizedUrl = resizedUrlImage.ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/trim/fit-in/200x400/left/bottom/smart/filters:grayscale():fill(blue):quality(100):format(webp):watermark(http://localhost/unsafe/-0x-0/https://localhost/watermark.png,0,10,50)/https://localhost/image.jpg");
        }

        [Test]
        public void ThumborSignedUrl()
        {
            var thumbor = new Thumbor("http://localhost/", "sample_key");
            var resizedUrl = thumbor.BuildImage("https://localhost/image.jpg").ToFullUrl();

            resizedUrl.Should().Be("http://localhost/_fak0PqFdoaKkMQpbxPE0ql8dtY=/https://localhost/image.jpg");
        }
    }
}
