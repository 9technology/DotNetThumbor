﻿namespace DotNetThumbor
{
    using System;
    using System.Collections.Generic;

    public class Thumbor : IThumbor
    {
        private readonly Uri thumborServerUrl;

        private Uri imageUrl;

        private string resizeWidthAndHeight;

        private bool smartImage;

        private ImageFormat outputFormat;

        private ImageHorizontalAlign horizontalAlign;

        private ImageVerticalAlign verticalAlign;

        private string cropCoordinates;

        private int? quality;

        private bool grayscale;

        private List<string> watermarks = new List<string>();

        private string fillColour;

        private bool trim;

        private string fitin;

        private bool flipImageHorizonal;

        private bool flipImageVertical;

        private int? width;

        private int? height;

        public Thumbor(string thumborServerUrl)
        {
            this.thumborServerUrl = new Uri(thumborServerUrl);
        }

        public enum ImageFormat
        {
            /// <summary>
            /// The none.
            /// </summary>
            None,

            /// <summary>
            /// The webp.
            /// </summary>
            Webp,

            /// <summary>
            /// The jpeg.
            /// </summary>
            Jpeg,

            /// <summary>
            /// The png.
            /// </summary>
            Png,

            /// <summary>
            /// The gif.
            /// </summary>
            Gif
        }

        public enum ImageHorizontalAlign
        {
            /// <summary>
            /// The center.
            /// </summary>
            Center,

            /// <summary>
            /// The left.
            /// </summary>
            Left,

            /// <summary>
            /// The right.
            /// </summary>
            Right
        }

        public enum ImageVerticalAlign
        {
            /// <summary>
            /// The middle.
            /// </summary>
            Middle,

            /// <summary>
            /// The top.
            /// </summary>
            Top,

            /// <summary>
            /// The bottom.
            /// </summary>
            Bottom
        }

        public Thumbor BuildImage(string imageUrl)
        {
            try
            {
                this.imageUrl = new Uri(imageUrl);
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException("Invalid URL", ex);
            }

            return this;
        }

        public Thumbor Resize(int? width, int? height)
        {
            this.width = width ?? 0;
            this.height = height ?? 0;

            return this;
        }

        public Thumbor Smart(bool smartImage)
        {
            this.smartImage = smartImage;
            return this;
        }

        public Thumbor Format(ImageFormat imageFormat)
        {
            this.outputFormat = imageFormat;
            return this;
        }

        public Thumbor Crop(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            this.cropCoordinates = string.Format("{0}x{1}:{2}x{3}", topLeft, topRight, bottomLeft, bottomRight);
            return this;
        }

        public Thumbor Quality(int? quality)
        {
            this.quality = quality;
            return this;
        }

        public Thumbor Grayscale(bool grayscaleImage)
        {
            this.grayscale = grayscaleImage;
            return this;
        }

        public Thumbor Watermark(string imageUrl, int right, int down, int transparency)
        {
            this.watermarks.Add(string.Format("watermark({0},{1},{2},{3})", imageUrl, right, down, transparency));
            return this;
        }

        public Thumbor Watermark(Thumbor thumborImage, int right, int down, int transparency)
        {
            return this.Watermark(thumborImage.ToUrl(), right, down, transparency);
        }

        public Thumbor Fill(string fillColour)
        {
            this.fillColour = fillColour;
            return this;
        }

        public Thumbor Trim(bool trimImage)
        {
            this.trim = trimImage;
            return this;
        }

        public Thumbor FitIn(bool fitIn)
        {
            this.fitin = fitIn ? "fit-in" : string.Empty;
            return this;
        }

        public Thumbor FullFitIn(bool fullFitIn)
        {
            this.fitin = fullFitIn ? "full-fit-in" : string.Empty;
            return this;
        }

        public Thumbor HorizontalAlign(ImageHorizontalAlign align)
        {
            this.horizontalAlign = align;
            return this;
        }

        public Thumbor VerticalAlign(ImageVerticalAlign align)
        {
            this.verticalAlign = align;
            return this;
        }

        public Thumbor HorizontalFlip(bool flipImageHorizontal)
        {
            this.flipImageHorizonal = flipImageHorizontal;
            return this;
        }

        public Thumbor VerticalFlip(bool flipImageVertical)
        {
            this.flipImageVertical = flipImageVertical;
            return this;
        }

        public override string ToString()
        {
            return this.ToUrl();
        }

        public string ToUrl()
        {
            if (this.imageUrl == null)
            {
                throw new InvalidOperationException("BuildImage must be called before ToUrl");
            }

            var url = this.thumborServerUrl + "unsafe/";

            if (this.trim)
            {
                url += "trim/";
            }

            if (!string.IsNullOrEmpty(this.cropCoordinates))
            {
                url += this.cropCoordinates + "/";
            }

            if (!string.IsNullOrEmpty(this.fitin))
            {
                url += this.fitin + "/";
            }

            if (this.width != null || this.height != null)
            {
                if (this.width == 0 && this.flipImageHorizonal)
                {
                    url += string.Format("-0x");
                }
                else if (this.flipImageHorizonal)
                {
                    this.width = this.width * -1;
                    url += string.Format("{0}x", this.width);
                }
                else
                {
                    url += string.Format("{0}x", this.width);
                }

                if (this.height == 0 && this.flipImageVertical)
                {
                    url += string.Format("-0/");
                }
                else if (this.flipImageVertical)
                {
                    this.height = this.height * -1;
                    url += string.Format("{0}/", this.height);
                }
                else
                {
                    url += string.Format("{0}/", this.height);
                }
            }

            if (this.horizontalAlign != ImageHorizontalAlign.Center)
            {
                url += this.horizontalAlign.ToString().ToLower() + "/";
            }

            if (this.verticalAlign != ImageVerticalAlign.Middle)
            {
                url += this.verticalAlign.ToString().ToLower() + "/";
            }

            if (this.smartImage)
            {
                url += "smart/";
            }

            var filters = new List<string>();
            if (this.outputFormat != ImageFormat.None)
            {
                filters.Add(string.Format("format({0})", this.outputFormat.ToString().ToLower()));
            }

            if (this.quality != null)
            {
                filters.Add(string.Format("quality({0})", this.quality));
            }

            if (this.grayscale)
            {
                filters.Add("grayscale()");
            }

            if (this.watermarks.Count != 0)
            {
                filters.AddRange(this.watermarks);
            }

            if (!string.IsNullOrEmpty(this.fillColour))
            {
                filters.Add(string.Format("fill({0})", this.fillColour));
            }

            if (filters.Count != 0)
            {
                url += "filters:" + string.Join(":", filters) + "/";
            }


            return string.Format("{0}{1}", url, this.imageUrl);
        }


    }
}
