namespace DotNetThumbor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class ThumborImage
    {
        private readonly List<string> watermarks = new List<string>();

        private readonly Dictionary<string, string> filters = new Dictionary<string, string>(); 

        private readonly ThumborSigner thumborSigner;

        private readonly string thumborSecretKey;

        private readonly Uri thumborServerUrl;

        private readonly string imageUrl;

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
            this.imageUrl = imageUrl;
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
        public ThumborImage Resize(int? newWidth, int? newHeight)
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
        public ThumborImage Smart(bool doSmartImage)
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
        public ThumborImage Format(Thumbor.ImageFormat imageFormat)
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
        public ThumborImage Crop(int topLeft, int topRight, int bottomLeft, int bottomRight)
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
        public ThumborImage Quality(int imageQuality)
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
        public ThumborImage Grayscale(bool grayscaleImage)
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
        /// <param name="right">
        /// How many pixels right the watermark should be. If the value is 'center' (without the single quotes), the watermark will be centered horizontally.
        /// If the value is 'repeat' (without the single quotes), the watermark will be repeated horizontally.
        /// If the value is a positive or negative number followed by a 'p' (ex. 20p), it will calculate the value from the image width as percentage.
        /// </param>
        /// <param name="down">
        /// How many pixels down the watermark should be. If the value is 'center' (without the single quotes), the watermark will be centered vertically.
        /// If the value is 'repeat' (without the single quotes), the watermark will be repeated vertically.
        /// If the value is a positive or negative number followed by a 'p' (ex. 20p), it will calculate the value from the image height as percentage.
        /// </param>
        /// <param name="transparency">Watermark image transparency 0 = opaque - 100 fully transparent</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Watermark(string watermarkImageUrl, string right, string down, int transparency)
        {
            this.watermarks.Add(string.Format("watermark({0},{1},{2},{3})", watermarkImageUrl, right, down, transparency));
            return this;
        }

        /// <summary>
        /// Sets the fill filter on this image.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="fillInColour"></param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Fill(string fillInColour)
        {
            this.ReplaceOrAddFilter("fill", fillInColour);
            return this;
        }

        /// <summary>
        /// Adds the trim option to this image. See https://github.com/thumbor/thumbor/wiki/Usage#trim for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageTrimOption">The trim option for this image.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Trim(Thumbor.ImageTrimOption imageTrimOption)
        {
            this.trim = imageTrimOption;
            return this;
        }

        /// <summary>
        /// Adds the fit in option to the image. See https://github.com/thumbor/thumbor/wiki/Usage#fit-in for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="fitIn">True to set fit-in and false to remove</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage FitIn(bool fitIn)
        {
            this.fitin = fitIn ? "fit-in" : string.Empty;
            return this;
        }

        /// <summary>
        /// Adds the full fit in option to the image. See https://github.com/thumbor/thumbor/wiki/Usage#fit-in for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="fullFitIn">True to set full-fit-in and false to remove</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage FullFitIn(bool fullFitIn)
        {
            this.fitin = fullFitIn ? "full-fit-in" : string.Empty;
            return this;
        }

        /// <summary>
        /// Sets horizontal alignment for the image. See https://github.com/thumbor/thumbor/wiki/Usage#horizontal-align for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="align">Value to set the alignment for the image.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage HorizontalAlign(Thumbor.ImageHorizontalAlign align)
        {
            this.horizontalAlign = align;
            return this;
        }

        /// <summary>
        /// Sets vertical alignment for the image. See https://github.com/thumbor/thumbor/wiki/Usage#horizontal-align for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="align">Value to set the alignment for the image.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage VerticalAlign(Thumbor.ImageVerticalAlign align)
        {
            this.verticalAlign = align;
            return this;
        }

        /// <summary>
        /// Flips the image horizontally. Generally you can set the width to negative but for keeping the orginal size IE 0 this cannot be done. This function
        /// is here for those cases.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="flipHorizontal">True to flip the image and false to keep it as is.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage HorizontalFlip(bool flipHorizontal)
        {
            this.width = this.width ?? 0;
            this.flipImageHorizonal = flipHorizontal;
            return this;
        }

        /// <summary>
        /// Flips the image vertically. Generally you can set the height to negative but for keeping the orginal size IE 0 this cannot be done. This function
        /// is here for those cases.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="flipVertical">True to flip the image and false to keep it as is.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage VerticalFlip(bool flipVertical)
        {
            this.height = this.height ?? 0;
            this.flipImageVertical = flipVertical;
            return this;
        }

        /// <summary>
        /// Sets the brightness filter for the image. See https://github.com/thumbor/thumbor/wiki/Brightness for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageBrightness">Value to set the brightness to.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Brightness(int imageBrightness)
        {
            this.ReplaceOrAddFilter("brightness", imageBrightness.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Sets the contrast filter for the image. See https://github.com/thumbor/thumbor/wiki/Contrast for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageContrast">Value to set the brightness to.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Contrast(int imageContrast)
        {
            this.ReplaceOrAddFilter("contrast", imageContrast.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Sets the colorize filter for the image. See https://github.com/thumbor/thumbor/wiki/Colorize for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="redPercentage">Percentage of red</param>
        /// <param name="greenPercentage">Percentage of green</param>
        /// <param name="bluePercentage">Percentage of blue</param>
        /// <param name="fillColor">Should be a 6 digit hex colour EG 004C9A</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Colorize(int redPercentage, int greenPercentage, int bluePercentage, string fillColor)
        {
            this.ReplaceOrAddFilter("colorize", string.Format("{0},{1},{2},{3}", redPercentage, greenPercentage, bluePercentage, fillColor));
            return this;
        }

        /// <summary>
        /// Sets the equalize filter for the image. See https://github.com/thumbor/thumbor/wiki/Equalize for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="equalizeImage">True to enable the filter, false to remove.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Equalize(bool equalizeImage)
        {
            if (!equalizeImage)
            {
                this.filters.Remove("equalize");
                return this;
            }

            this.ReplaceOrAddFilter("equalize", string.Empty);
            return this;
        }

        /// <summary>
        /// Sets the max_bytes filter for the image. See https://github.com/thumbor/thumbor/wiki/Max-bytes for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageMaxBytes">Integer value which is the number of bytes</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage MaxBytes(int imageMaxBytes)
        {
            this.ReplaceOrAddFilter("max_bytes", imageMaxBytes.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Sets the noise filter for the image. See https://github.com/thumbor/thumbor/wiki/Noise for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageNoise">Amount of noise to be used</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Noise(int imageNoise)
        {
            this.ReplaceOrAddFilter("noise", imageNoise.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Sets the no_updscale filter for the image. See https://github.com/thumbor/thumbor/wiki/No-Upscale for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="noUpscaleImage">True to set the filter and false to remove.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage NoUpscale(bool noUpscaleImage)
        {
            if (!noUpscaleImage)
            {
                this.filters.Remove("no_upscale");
                return this;
            }

            this.ReplaceOrAddFilter("no_upscale", string.Empty);
            return this;
        }

        /// <summary>
        /// Sets the RBG filter for the image. See https://github.com/thumbor/thumbor/wiki/Rgb for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="red">Redness to have in the image.</param>
        /// <param name="green">Greeness to have in the image.</param>
        /// <param name="blue">Blueness to have in the image.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Rgb(int red, int green, int blue)
        {
            this.ReplaceOrAddFilter("rgb", string.Format("{0},{1},{2}", red, green, blue));
            return this;
        }

        /// <summary>
        /// Sets the round corners filter for the image. See https://github.com/thumbor/thumbor/wiki/Round-corners for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="radiusA"></param>
        /// <param name="radiusB"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage RoundCorners(int radiusA, int? radiusB, int red, int green, int blue)
        {
            var radiusValue = radiusB == null
                                  ? radiusA.ToString(CultureInfo.InvariantCulture)
                                  : string.Format("{0}|{1}", radiusA, radiusB);
            this.ReplaceOrAddFilter("round_corners", string.Format("{0},{1},{2},{3}", radiusValue, red, green, blue));
            return this;
        }

        /// <summary>
        /// Sets the rotate filter for the image. See https://github.com/thumbor/thumbor/wiki/Rotate for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageRotateAngle">int value which is the number of degrees to rotate</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Rotate(int imageRotateAngle)
        {
            this.ReplaceOrAddFilter("rotate", imageRotateAngle.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Sets the saturation filter for the image. See https://github.com/thumbor/thumbor/wiki/Saturation for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageSaturation">percentage of the saturation to apply</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Saturation(double imageSaturation)
        {
            this.ReplaceOrAddFilter("saturation", imageSaturation.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Sets the sharpen filter for the image. See https://github.com/thumbor/thumbor/wiki/Sharpen for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="sharpenAmount">Sharpen amount.</param>
        /// <param name="sharpenRadius">Sharpen radious</param>
        /// <param name="luminance">true to sharpen only the luminance channel, false to sharpen all.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Sharpen(double sharpenAmount, double sharpenRadius, bool luminance)
        {
            this.ReplaceOrAddFilter("sharpen", string.Format("{0},{1},{2}", sharpenAmount, sharpenRadius, luminance.ToString().ToLower()));
            return this;
        }

        /// <summary>
        /// Sets the strip_icc filter for the image. See https://github.com/thumbor/thumbor/wiki/Strip-icc for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="stripIccFromImage">true to add the filter and false to remove.</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage StripIcc(bool stripIccFromImage)
        {
            if (!stripIccFromImage)
            {
                this.filters.Remove("strip_icc");
                return this;
            }

            this.ReplaceOrAddFilter("strip_icc", string.Empty);
            return this;
        }

        /// <summary>
        /// Sets the convolution filter for the image. See https://github.com/thumbor/thumbor/wiki/Convolution for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="matrix">matrix items for the convolution filter</param>
        /// <param name="columns">number of columns in the matrix</param>
        /// <param name="shouldNormalise">should the divide by sum of all items</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Convolution(IList<int> matrix, int columns, bool shouldNormalise)
        {
            this.ReplaceOrAddFilter("convolution", string.Format("{0},{1},{2}", string.Join(";", matrix), columns, shouldNormalise.ToString().ToLower()));
            return this;
        }

        /// <summary>
        /// Sets the blue filter for the image. See https://github.com/thumbor/thumbor/wiki/Blur for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="blurRadius">sets the radius of the blur.</param>
        /// <param name="blurSigma">sets the sigma for the blur, if null passed in will be ignored</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Blur(int blurRadius, int? blurSigma)
        {
            var blurParamater = blurSigma == null ? blurRadius.ToString(CultureInfo.InvariantCulture) : blurRadius + "," + blurSigma;
            this.ReplaceOrAddFilter("blur", blurParamater);
            return this;
        }

        /// <summary>
        /// Sets the extract focal filter for the image. See https://github.com/thumbor/thumbor/wiki/Extract-Focal-Points for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage ExtractFocal()
        {
            this.ReplaceOrAddFilter("extract_focal", string.Empty);
            return this;
        }

        /// <summary>
        /// Sets the gifv filter for the image. See https://github.com/thumbor/thumbor/wiki/GifV for details.
        /// Can be called multiple times with the last call overriding all previous calls.
        /// </summary>
        /// <param name="imageGifVOption">Set to none to get mp4 otherwise webm for webm output</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage GifV(Thumbor.ImageGifVOption imageGifVOption)
        {
            var gifvParameter = imageGifVOption == Thumbor.ImageGifVOption.None
                                    ? string.Empty
                                    : imageGifVOption.ToString().ToLower();
            this.ReplaceOrAddFilter("gifv", gifvParameter);
            return this;
        }

        /// <summary>
        /// Sets the RGB curve filter for the image. See https://github.com/thumbor/thumbor/wiki/Curve for details.
        /// </summary>
        /// <param name="curveAll">Values for all channels</param>
        /// <param name="curveRed">Values for red channels</param>
        /// <param name="curveGreen">Values for green channels</param>
        /// <param name="curveBlue">Values for blue channels</param>
        /// <returns>The current thumbor image object.</returns>
        public ThumborImage Curve(IList<Tuple<int, int>> curveAll, IList<Tuple<int, int>> curveRed, IList<Tuple<int, int>> curveGreen, IList<Tuple<int, int>> curveBlue)
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

        /// <summary>
        /// An alias to ToUrl
        /// </summary>
        /// <returns>The results of ToUrl</returns>
        public override string ToString()
        {
            return this.ToUrl();
        }

        /// <summary>
        /// Returns an unsafe url which is not signed.
        /// </summary>
        /// <returns>The URL with the unsafe URL</returns>
        public string ToUnsafeUrl()
        {
            var unsafeImageUrl = string.Format("unsafe/{0}", this.FormatUrlParts());
            return unsafeImageUrl;
        }

        /// <summary>
        /// Returns either a signed url if a thumbor secret key is set otherwise an unsafe url.
        /// </summary>
        /// <returns>Signed or unsigned URL for the image</returns>
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
            var signedImageUrl = string.Format("{0}/{1}", signedKey, urlparts);

            return signedImageUrl;
        }

        /// <summary>
        /// Returns full url including the thumbor server.
        /// </summary>
        /// <returns>Signed or unsigned URL for the image</returns>
        public string ToFullUrl()
        {
            var fullUrl = string.Format("{0}{1}", this.thumborServerUrl, this.ToUrl());
            return fullUrl;
        }

        /// <summary>
        /// The URL part order is important! Do not change it without consulting the Thumbor documentation
        /// which can be found on github https://github.com/thumbor/thumbor/wiki
        /// </summary>
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
