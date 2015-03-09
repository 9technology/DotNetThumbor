namespace DotNetThumbor
{
    using System;

    public class Thumbor : IThumbor
    {
        private readonly Uri thumborServerUrl;

        public Thumbor(string thumborServerUrl)
        {
            this.thumborServerUrl = new Uri(thumborServerUrl);
        }

        public void BuildImage(string imageUrl)
        {
            throw new System.NotImplementedException();
        }

        public string ToUrl()
        {
            throw new System.NotImplementedException();
        }
    }
}
