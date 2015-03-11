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
                this.ReplaceOrAddFilter("format", imageFormat.ToString().ToLower());
            }

            return this;
        }

        public IThumborImage Crop(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            this.cropCoordinates = string.Format("{0}x{1}:{2}x{3}", topLeft, topRight, bottomLeft, bottomRight);
            return this;
        }

        public IThumborImage Quality(int imageQuality)
        {
            this.ReplaceOrAddFilter("quality", imageQuality.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        public IThumborImage Grayscale(bool grayscaleImage)
        {
            if (!grayscaleImage)
            {
                this.filters.Remove("grayscale");
                return this;
            }

            this.ReplaceOrAddFilter("grayscale", string.Empty);
            return this;
        }

        public IThumborImage Watermark(string watermarkImageUrl, int right, int down, int transparency)
        {
            this.watermarks.Add(string.Format("watermark({0},{1},{2},{3})", watermarkImageUrl, right, down, transparency));
            return this;
        }

        public IThumborImage Fill(string fillInColour)
        {
            this.ReplaceOrAddFilter("fill", fillInColour);
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

        public IThumborImage Brightness(int imageBrightness)
        {
            this.ReplaceOrAddFilter("brightness", imageBrightness.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        public IThumborImage Contrast(int imageContrast)
        {
            this.ReplaceOrAddFilter("contrast", imageContrast.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        public IThumborImage Colorize(int redPercentage, int greenPercentage, int bluePercentage, string fillColor)
        {
            this.ReplaceOrAddFilter("colorize", string.Format("{0},{1},{2},{3}", redPercentage, greenPercentage, bluePercentage, fillColor));
            return this;
        }

        public IThumborImage Equalize(bool equalizeImage)
        {
            if (!equalizeImage)
            {
                this.filters.Remove("equalize");
                return this;
            }

            this.ReplaceOrAddFilter("equalize", string.Empty);
            return this;
        }

        public IThumborImage MaxBytes(int imageMaxBytes)
        {
            this.ReplaceOrAddFilter("max_bytes", imageMaxBytes.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        public IThumborImage Noise(int imageNoise)
        {
            this.ReplaceOrAddFilter("noise", imageNoise.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        public IThumborImage NoUpscale(bool noUpscaleImage)
        {
            if (!noUpscaleImage)
            {
                this.filters.Remove("no_upscale");
                return this;
            }

            this.ReplaceOrAddFilter("no_upscale", string.Empty);
            return this;
        }

        public IThumborImage Rgb(int red, int green, int blue)
        {
            this.ReplaceOrAddFilter("rgb", string.Format("{0},{1},{2}", red, green, blue));
            return this;
        }

        public IThumborImage RoundCorners(int radiusA, int? radiusB, int red, int green, int blue)
        {
            var radiusValue = radiusB == null
                                  ? radiusA.ToString(CultureInfo.InvariantCulture)
                                  : string.Format("{0}|{1}", radiusA, radiusB);
            this.ReplaceOrAddFilter("round_corners", string.Format("{0},{1},{2},{3}", radiusValue, red, green, blue));
            return this;
        }

        public IThumborImage Rotate(int imageRotateAngle)
        {
            this.ReplaceOrAddFilter("rotate", imageRotateAngle.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        public IThumborImage Saturation(double imageSaturation)
        {
            this.ReplaceOrAddFilter("saturation", imageSaturation.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        public IThumborImage Sharpen(double sharpenAmount, double sharpenRadius, bool luminance)
        {
            this.ReplaceOrAddFilter("sharpen", string.Format("{0},{1},{2}", sharpenAmount, sharpenRadius, luminance.ToString().ToLower()));
            return this;
        }

        public IThumborImage StripIcc(bool stripIccFromImage)
        {
            if (!stripIccFromImage)
            {
                this.filters.Remove("strip_icc");
                return this;
            }

            this.ReplaceOrAddFilter("strip_icc", string.Empty);
            return this;
        }

        public IThumborImage Convolution(IList<int> matrix, int columns, bool shouldNormalise)
        {
            this.ReplaceOrAddFilter("convolution", string.Format("{0},{1},{2}", string.Join(";", matrix), columns, shouldNormalise.ToString().ToLower()));
            return this;
        }

        public IThumborImage Blur(int blurRadius, int? blurSigma)
        {
            var blurParamater = blurSigma == null ? blurRadius.ToString(CultureInfo.InvariantCulture) : blurRadius + "," + blurSigma;
            this.ReplaceOrAddFilter("blur", blurParamater);
            return this;
        }

        public IThumborImage ExtractFocal()
        {
            this.ReplaceOrAddFilter("extract_focal", string.Empty);
            return this;
        }

        public IThumborImage GifV(Thumbor.ImageGifVOption imageGifVOption)
        {
            var gifvParameter = imageGifVOption == Thumbor.ImageGifVOption.None
                                    ? string.Empty
                                    : imageGifVOption.ToString().ToLower();
            this.ReplaceOrAddFilter("gifv", gifvParameter);
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

            this.ReplaceOrAddFilter("curve", curve);
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

            var methodFilters = this.filters.Select(x => string.Format("{0}({1})", x.Key, x.Value)).ToList();
            methodFilters.AddRange(this.watermarks);

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
