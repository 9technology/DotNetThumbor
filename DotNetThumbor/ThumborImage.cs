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

        private Thumbor.ImageTrimOption trim;

        private string fitin;

        private bool flipImageHorizonal;

        private bool flipImageVertical;

        private int? width;

        private int? height;

        /// <summary>
        /// Creates a new ThumborImage which uses the provided parameters to create either a
        /// signed or unsigned URL with filters and other thumbor options
        /// </summary>
        /// <param name="thumborSigner">Implementation of IThumborSigner for signing keys</param>
        /// <param name="thumborServerUrl">URL to the thumbor server EG http://mythumborserver.com/ </param>
        /// <param name="thumborSecretKey">The secret key used by the thumbor server for signing URL's</param>
        /// <param name="imageUrl">URL to the image that will be manipulated by Thumbor</param>
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

        /// <summary>
        /// Resize the image. See https://github.com/thumbor/thumbor/wiki/Usage#image-size for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="newWidth">Width to resize the image to. Nulls will be treated as 0. Negative numbers will flip the image.</param>
        /// <param name="newHeight">Height to resize the image to. Nulls will be treated as 0. Negative numbers will flip the image.</param>
        /// <returns>The current thumbor image object.</returns>
        public IThumborImage Resize(int? newWidth, int? newHeight)
        {
            this.width = newWidth ?? 0;
            this.height = newHeight ?? 0;

            return this;
        }

        /// <summary>
        /// Enables or disables smart cropping on the image. See https://github.com/thumbor/thumbor/wiki/Usage#smart-cropping for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="doSmartImage">True to enable smart cropping and false to remove it.</param>
        /// <returns>The current thumbor image object.</returns>
        public IThumborImage Smart(bool doSmartImage)
        {
            this.smartImage = doSmartImage;
            return this;
        }

        /// <summary>
        /// Sets the output format of the image. See https://github.com/thumbor/thumbor/wiki/Format for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageFormat">Image format that should be specified.</param>
        /// <returns>The current thumbor image object.</returns>
        public IThumborImage Format(Thumbor.ImageFormat imageFormat)
        {
            if (imageFormat != Thumbor.ImageFormat.None)
            {
                this.ReplaceOrAddFilter("format", imageFormat.ToString().ToLower());
            }

            return this;
        }

        /// <summary>
        /// Crop the image around the points specified. See https://github.com/thumbor/thumbor/wiki/Usage#manual-crop for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="topLeft">Top left pixel location to start the crop.</param>
        /// <param name="topRight">Top right pixel location to start the crop.</param>
        /// <param name="bottomLeft">Bottom left pixel location to start the crop.</param>
        /// <param name="bottomRight">Bottom right pixel location to start the crop.</param>
        /// <returns>The current thumbor image object.</returns>
        public IThumborImage Crop(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            this.cropCoordinates = string.Format("{0}x{1}:{2}x{3}", topLeft, topRight, bottomLeft, bottomRight);
            return this;
        }

        /// <summary>
        /// Sets the quality of the ouput image. See https://github.com/thumbor/thumbor/wiki/Quality for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageQuality">Value between 0 and 100 of the quality to use</param>
        /// <returns>The current thumbor image object.</returns>
        public IThumborImage Quality(int imageQuality)
        {
            this.ReplaceOrAddFilter("quality", imageQuality.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Enables or disables the greyscale filter. See https://github.com/thumbor/thumbor/wiki/Grayscale for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="grayscaleImage">True to enable the filter and false to remove it.</param>
        /// <returns>The current thumbor image object.</returns>
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

        /// <summary>
        /// Adds a watermark image to this image. The watermark image can be the output of another thumbor image. See https://github.com/thumbor/thumbor/wiki/Watermark for details.
        /// Can be called multiple times with each watermark being included on the base image. The last added watermark 
        /// will overlay all previous watermarks if there is an overlap.
        /// </summary>
        /// <param name="watermarkImageUrl">URL to the image to use as a watermark. Can be the output of another thumbor image.</param>
        /// <param name="right">How many pixels right the watermark should be.</param>
        /// <param name="down">How many pixels down the watermark should be.</param>
        /// <param name="transparency">Watermark image transparency 0 = opaque - 100 fully transparent</param>
        /// <returns></returns>
        public IThumborImage Watermark(string watermarkImageUrl, int right, int down, int transparency)
        {
            this.watermarks.Add(string.Format("watermark({0},{1},{2},{3})", watermarkImageUrl, right, down, transparency));
            return this;
        }

        /// <summary>
        /// 
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="fillInColour"></param>
        /// <returns></returns>
        public IThumborImage Fill(string fillInColour)
        {
            this.ReplaceOrAddFilter("fill", fillInColour);
            return this;
        }

        public IThumborImage Trim(Thumbor.ImageTrimOption imageTrimOption)
        {
            this.trim = imageTrimOption;
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

        public override string ToString()
        {
            return this.ToUrl();
        }

        public string ToUnsafeUrl()
        {
            var unsafeImageUrl = string.Format("{0}unsafe/{1}", this.thumborServerUrl, this.FormatUrlParts());
            return unsafeImageUrl;
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
            var signedKey = this.thumborSigner.Encode(urlparts, this.thumborSecretKey);
            var signedImageUrl = string.Format("{0}{1}/{2}", this.thumborServerUrl, signedKey, urlparts);

            return signedImageUrl;
        }

        private string FormatUrlParts()
        {
            var urlParts = new List<string>();

            if (this.trim != Thumbor.ImageTrimOption.None)
            {
                urlParts.Add(this.trim == Thumbor.ImageTrimOption.BottomRight ? "trim:bottom-right" : "trim");
            }
            
            if (!string.IsNullOrEmpty(this.cropCoordinates))
            {
                urlParts.Add(this.cropCoordinates);
            }

            if (!string.IsNullOrEmpty(this.fitin))
            {
                urlParts.Add(this.fitin);
            }

            var widthHeight = this.GetWidthAndHeight();
            if (widthHeight != null)
            {
                urlParts.Add(widthHeight);
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
                urlParts.Add(string.Format("filters:{0}", string.Join(":", methodFilters)));
            }

            urlParts.Add(this.imageUrl.ToString());

            return string.Join("/", urlParts);
        }

        private string GetWidthAndHeight()
        {
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

                return string.Format("{0}x{1}", widthString, heightString);
            }

            return null;
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
