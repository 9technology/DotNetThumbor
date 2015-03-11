namespace DotNetThumbor
{
    using System;

    public class Thumbor : IThumbor
    {
        /// <summary>
        /// The thumbor secret key.
        /// </summary>
        private readonly string thumborSecretKey;

        /// <summary>
        /// The thumbor server url.
        /// </summary>
        private readonly Uri thumborServerUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Thumbor"/> class. 
        /// Creates a thumbor factory without signed image URL's
        /// </summary>
        /// <param name="thumborServerUrl">
        /// URL to the thumbor server EG http://mythumborserver.com/ 
        /// </param>
        public Thumbor(string thumborServerUrl)
        {
            this.thumborServerUrl = new Uri(thumborServerUrl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thumbor"/> class. 
        /// Creates a thumbor factory which supports signed image URL's using the supplied key which should be the same
        /// as the targetted thumbor server
        /// </summary>
        /// <param name="thumborServerUrl">
        /// </param>
        /// <param name="thumborSecretKey">
        /// </param>
        public Thumbor(string thumborServerUrl, string thumborSecretKey)
        {
            this.thumborSecretKey = thumborSecretKey;
            this.thumborServerUrl = new Uri(thumborServerUrl);
        }

        public enum ImageFormat
        {
            /// <summary>
            /// Default. When set will preseve the source format to be the same as the input.
            /// </summary>
            None,

            /// <summary>
            /// Change the output image format to be a WebP image.
            /// </summary>
            Webp,

            /// <summary>
            /// Change the output image format to be a JPEG image.
            /// </summary>
            Jpeg,

            /// <summary>
            /// Change the output image format to be a PNG image.
            /// </summary>
            Png,

            /// <summary>
            /// Change the output image format to be a GIF image.
            /// </summary>
            Gif
        }

        public enum ImageHorizontalAlign
        {
            /// <summary>
            /// Default. Sets the crop / resize point to be the center of the image.
            /// </summary>
            Center,

            /// <summary>
            /// Sets the crop / resize point to be the left of the image.
            /// </summary>
            Left,

            /// <summary>
            /// Sets the crop / resize point to be the right of the image.
            /// </summary>
            Right
        }

        public enum ImageVerticalAlign
        {
            /// <summary>
            /// Default. Sets the crop / resize point to be the middle of the image.
            /// </summary>
            Middle,

            /// <summary>
            /// Sets the crop / resize point to be the top of the image.
            /// </summary>
            Top,

            /// <summary>
            /// Sets the crop / resize point to be the bottom of the image.
            /// </summary>
            Bottom
        }

        public enum ImageGifVOption
        {
            /// <summary>
            /// Default. Convert gif to mp4.
            /// </summary>
            None,

            /// <summary>
            /// Convert gif to webm.
            /// </summary>
            Webm
        }

        public enum ImageTrimOption
        {
            /// <summary>
            /// Default. No trim option.
            /// </summary>
            None,

            /// <summary>
            /// Sets trim to use the top-left pixel colour.
            /// </summary>
            TopLeft,

            /// <summary>
            /// Sets trim to use the bottom-right pixel colour.
            /// </summary>
            BottomRight
        }

        /// <summary>
        /// Builds the thumbor image url based on the URL supplied and returns an thumbor image
        /// to which thumbor operations can be applied to.
        /// </summary>
        /// <param name="imageUrl">URL to an image which thumbor need to be applied to</param>
        /// <returns>Implementation of a thumbor image which thumber operations can be applied to</returns>
        public IThumborImage BuildImage(string imageUrl)
        {
            return new ThumborImage(new ThumborSigner(), this.thumborServerUrl, this.thumborSecretKey, imageUrl);
        }       
    }
}
