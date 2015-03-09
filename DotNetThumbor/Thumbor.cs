namespace DotNetThumbor
{
    using System;

    public class Thumbor : IThumbor
    {
        private readonly Uri thumborServerUrl;

        private Uri imageUrl;

        public Thumbor(string thumborServerUrl)
        {
            this.thumborServerUrl = new Uri(thumborServerUrl);
        }

        public Thumbor BuildImage(string imageUrl)
        {
            this.imageUrl = new Uri(imageUrl);
            return this;
        }

        public string ToUrl()
        {
            if (this.imageUrl == null)
            {
                throw new InvalidOperationException("BuildImage must be called before ToUrl");
            }

            return string.Format("{0}unsafe/{1}", this.thumborServerUrl, this.imageUrl);
        }
    }
}
