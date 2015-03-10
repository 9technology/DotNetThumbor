namespace DotNetThumbor
{
    using System;

    public class Thumbor : IThumbor
    {
        private readonly string thumborSecretKey;

        private readonly Uri thumborServerUrl;

        public Thumbor(string thumborServerUrl)
        {
            this.thumborServerUrl = new Uri(thumborServerUrl);
        }

        public Thumbor(string thumborServerUrl, string thumborSecretKey)
        {
            this.thumborSecretKey = thumborSecretKey;
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

        public ThumborImage BuildImage(string imageUrl)
        {
            return new ThumborImage(this.thumborServerUrl, this.thumborSecretKey, imageUrl);
        }       
    }
}
