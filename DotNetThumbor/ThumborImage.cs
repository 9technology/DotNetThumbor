namespace DotNetThumbor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class ThumborImage : IThumborImage
    {
        private readonly List<string> watermarks = new List<string>();

        private readonly Dictionary<string, string> filters = new Dictionary<string, string>(); 

        private readonly ThumborSigner thumborSigner;

        private readonly string thumborSecretKey;

        private readonly Uri thumborServerUrl;

        private readonly Uri imageUrl;

        private bool smartImage;

        private Thumbor.ImageHorizontalAlign horizontalAlign;

        private Thumbor.ImageVerticalAlign verticalAlign;

        private string cropCoordinates;

        private bool trim;

        private string fitin;

        private bool flipImageHorizonal;

        private bool flipImageVertical;

        private int? width;

        private int? height;

        public ThumborImage(ThumborSigner thumborSigner, Uri thumborServerUrl, string thumborSecretKey, string imageUrl)
        {
            try
            {
                this.imageUrl = new Uri(imageUrl);
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException("Invalid URL", ex);
            }

            this.thumborSigner = thumborSigner;
            this.thumborSecretKey = thumborSecretKey;
            this.thumborServerUrl = thumborServerUrl;
        }

        public IThumborImage Resize(int? newWidth, int? newHeight)
        {
            this.width = newWidth ?? 0;
            this.height = newHeight ?? 0;

            return this;
        }

        public IThumborImage Smart(bool doSmartImage)
        {
            this.smartImage = doSmartImage;
            return this;
        }

        public IThumborImage Format(Thumbor.ImageFormat imageFormat)
        {
            if (imageFormat != Thumbor.ImageFormat.None)
            {
                this.filters.Add("format", string.Format("{0}", imageFormat.ToString().ToLower()));
            }

            return this;
        }

        public IThumborImage Crop(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            this.cropCoordinates = string.Format("{0}x{1}:{2}x{3}", topLeft, topRight, bottomLeft, bottomRight);
            return this;
        }

        public IThumborImage Quality(int? imageQuality)
        {
            this.filters.Add("quality", string.Format("{0}", imageQuality));
            return this;
        }

        public IThumborImage Grayscale(bool grayscaleImage)
        {
            this.filters.Add("grayscale", string.Empty);
            return this;
        }

        public IThumborImage Watermark(string watermarkImageUrl, int right, int down, int transparency)
        {
            this.watermarks.Add(string.Format("watermark({0},{1},{2},{3})", watermarkImageUrl, right, down, transparency));
            return this;
        }

        public IThumborImage Fill(string fillInColour)
        {
            this.filters.Add("fill", string.Format("{0}", fillInColour));
            return this;
        }

        public IThumborImage Trim(bool trimImage)
        {
            this.trim = trimImage;
            return this;
        }

        public IThumborImage FitIn(bool fitIn)
        {
            this.fitin = fitIn ? "fit-in" : string.Empty;
            return this;
        }

        public IThumborImage FullFitIn(bool fullFitIn)
        {
            this.fitin = fullFitIn ? "full-fit-in" : string.Empty;
            return this;
        }

        public IThumborImage HorizontalAlign(Thumbor.ImageHorizontalAlign align)
        {
            this.horizontalAlign = align;
            return this;
        }

        public IThumborImage VerticalAlign(Thumbor.ImageVerticalAlign align)
        {
            this.verticalAlign = align;
            return this;
        }

        public IThumborImage HorizontalFlip(bool flipHorizontal)
        {
            this.width = this.width ?? 0;
            this.flipImageHorizonal = flipHorizontal;
            return this;
        }

        public IThumborImage VerticalFlip(bool flipVertical)
        {
            this.height = this.height ?? 0;
            this.flipImageVertical = flipVertical;
            return this;
        }

        public override string ToString()
        {
            return this.ToUrl();
        }

        public string ToUnsafeUrl()
        {
            var server = this.thumborServerUrl + "unsafe/";
            return server + this.FormatUrlParts();
        }

        public IThumborImage Brightness(int? imageBrightness)
        {
            if (imageBrightness == null)
            {
                this.filters.Remove("brightness");
                return this;
            }

            this.ReplaceOrAddFilter("brightness", imageBrightness.ToString());
            return this;
        }

        public IThumborImage Contrast(int? imageContrast)
        {
            if (imageContrast == null)
            {
                this.filters.Remove("contrast");
                return this;
            }

            this.ReplaceOrAddFilter("contrast", imageContrast.ToString());
            return this;
        }

        public IThumborImage Colorize(int redPercentage, int greenPercentage, int bluePercentage, string fillColor)
        {
            this.filters.Add("colorize", string.Format("{0},{1},{2},{3}", redPercentage, greenPercentage, bluePercentage, fillColor));
            return this;
        }

        public IThumborImage Equalize(bool equalizeImage)
        {
            this.filters.Add("equalize", string.Empty);
            return this;
        }

        public IThumborImage MaxBytes(int? imageMaxBytes)
        {
            this.filters.Add("max_bytes", string.Format("{0}", imageMaxBytes));
            return this;
        }

        public IThumborImage Noise(int? imageNoise)
        {
            this.filters.Add("noise", string.Format("{0}", imageNoise));
            return this;
        }

        public IThumborImage NoUpscale(bool noUpscaleImage)
        {
            this.filters.Add("no_upscale", string.Empty);
            return this;
        }

        public IThumborImage Rgb(int red, int green, int blue)
        {
            this.filters.Add("rgb", string.Format("{0},{1},{2}", red, green, blue));
            return this;
        }

        public IThumborImage RoundCorners(int radiusA, int? radiusB, int red, int green, int blue)
        {
            this.filters.Add("round_corners", string.Format(
                "{0},{1},{2},{3}",
                radiusB == null ? radiusA.ToString(CultureInfo.InvariantCulture) : string.Format("{0}|{1}", radiusA, radiusB),
                red,
                green,
                blue));
            return this;
        }

        public IThumborImage Rotate(int? imageRotateAngle)
        {
            this.filters.Add("rotate", string.Format("{0}", imageRotateAngle));
            return this;
        }

        public IThumborImage Saturation(double? imageSaturation)
        {
            this.filters.Add("saturation", string.Format("{0}", imageSaturation));
            return this;
        }

        public IThumborImage Sharpen(double sharpenAmount, double sharpenRadius, bool luminance)
        {
            this.filters.Add("sharpen", string.Format(
                "{0},{1},{2}", sharpenAmount, sharpenRadius, luminance.ToString().ToLower()));
            return this;
        }

        public IThumborImage StripIcc(bool stripIccFromImage)
        {
            this.filters.Add("strip_icc", string.Empty);
            return this;
        }

        public IThumborImage Convolution(IList<int> matrix, int columns, bool shouldNormalise)
        {
            this.filters.Add("convolution", string.Format("{0},{1},{2}", string.Join(";", matrix), columns, shouldNormalise.ToString().ToLower()));
            return this;
        }

        public IThumborImage Blur(int blurRadius, int? blurSigma)
        {
            this.filters.Add("blur", string.Format("{0}", blurSigma == null ? blurRadius.ToString(CultureInfo.InvariantCulture) : blurRadius + "," + blurSigma));
            return this;
        }

        public IThumborImage ExtractFocal()
        {
            this.filters.Add("extract_focal", string.Empty);
            return this;
        }

        public IThumborImage GifV(Thumbor.ImageGifVOption imageGifVOption)
        {
            this.filters.Add("gifv", string.Format(
                "{0}",
                imageGifVOption == Thumbor.ImageGifVOption.None ? string.Empty : imageGifVOption.ToString().ToLower()));
            return this;
        }

        public IThumborImage Curve(IList<Tuple<int, int>> curveAll, IList<Tuple<int, int>> curveRed, IList<Tuple<int, int>> curveGreen, IList<Tuple<int, int>> curveBlue)
        {
            var curve = string.Format(
                "[{0}],[{1}],[{2}],[{3}]",
                string.Join(",",   curveAll.Select(x => string.Format("({0},{1})", x.Item1, x.Item2))),
                string.Join(",",   curveRed.Select(x => string.Format("({0},{1})", x.Item1, x.Item2))), 
                string.Join(",", curveGreen.Select(x => string.Format("({0},{1})", x.Item1, x.Item2))), 
                string.Join(",",  curveBlue.Select(x => string.Format("({0},{1})", x.Item1, x.Item2))));

            this.filters.Add("curve", curve);
            return this;
        }

        public string ToUrl()
        {
            if (this.imageUrl == null)
            {
                throw new InvalidOperationException("BuildImage must be called before ToUrl");
            }

            if (string.IsNullOrEmpty(this.thumborSecretKey))
            {
                return this.ToUnsafeUrl();
            }

            var urlparts = this.FormatUrlParts();
            var server = this.thumborServerUrl + this.thumborSigner.Encode(urlparts, this.thumborSecretKey) + "/";

            return server + urlparts;
        }

        private string FormatUrlParts()
        {
            var urlParts = new List<string>();

            if (this.trim)
            {
                urlParts.Add("trim");
            }

            if (!string.IsNullOrEmpty(this.cropCoordinates))
            {
                urlParts.Add(this.cropCoordinates);
            }

            if (!string.IsNullOrEmpty(this.fitin))
            {
                urlParts.Add(this.fitin);
            }

            if (this.width != null || this.height != null)
            {
                string widthString;
                if (this.width == 0 && this.flipImageHorizonal)
                {
                    widthString = "-0";
                }
                else if (this.flipImageHorizonal)
                {
                    this.width = this.width * -1;
                    widthString = this.width.ToString();
                }
                else
                {
                    widthString = this.width.ToString();
                }

                string heightString;
                if (this.height == 0 && this.flipImageVertical)
                {
                    heightString = "-0";
                }
                else if (this.flipImageVertical)
                {
                    this.height = this.height * -1;
                    heightString = this.height.ToString();
                }
                else
                {
                    heightString = this.height.ToString();
                }

                urlParts.Add(widthString + "x" + heightString);
            }

            if (this.horizontalAlign != Thumbor.ImageHorizontalAlign.Center)
            {
                urlParts.Add(this.horizontalAlign.ToString().ToLower());
            }

            if (this.verticalAlign != Thumbor.ImageVerticalAlign.Middle)
            {
                urlParts.Add(this.verticalAlign.ToString().ToLower());
            }

            if (this.smartImage)
            {
                urlParts.Add("smart");
            }

            var methodFilters = this.filters.Select(x => string.Format("{0}({1})", x.Key, x.Value)).ToArray();

            if (methodFilters.Count() != 0)
            {
                urlParts.Add("filters:" + string.Join(":", methodFilters));
            }

            urlParts.Add(this.imageUrl.ToString());

            return string.Join("/", urlParts);
        }

        private void ReplaceOrAddFilter(string filterName, string filterOptions)
        {
            if (this.filters.ContainsKey(filterName))
            {
                this.filters.Remove(filterName);
            }

            this.filters.Add(filterName, filterOptions);
        }
    }
}
