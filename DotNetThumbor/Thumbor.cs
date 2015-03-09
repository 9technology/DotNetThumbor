namespace DotNetThumbor
{
    using System;

    public class Thumbor : IThumbor
    {
        private readonly Uri thumborServerUrl;

        private Uri imageUrl;

        private string resizeWidthAndHeight;

        private bool beSmart;

        public Thumbor(string thumborServerUrl)
        {
            this.thumborServerUrl = new Uri(thumborServerUrl);
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
            width = width ?? 0;
            height = height ?? 0;

            this.resizeWidthAndHeight = width + "x" + height;
            return this;
        }

        public Thumbor Smart(bool beSmart)
        {
            this.beSmart = beSmart;
            return this;
        }

        public string ToUrl()
        {
            if (this.imageUrl == null)
            {
                throw new InvalidOperationException("BuildImage must be called before ToUrl");
            }

            var url = this.thumborServerUrl + "unsafe/";

            if (this.resizeWidthAndHeight != null)
            {
                url += this.resizeWidthAndHeight + "/";
            }

            if (this.beSmart)
            {
                url += "smart/";
            }

            return string.Format("{0}{1}", url, this.imageUrl);
        }


    }
}
