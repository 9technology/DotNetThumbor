namespace DotNetThumbor
{
    public interface IThumbor
    {
        Thumbor BuildImage(string imageUrl);

        Thumbor Resize(int? width, int? height);

        Thumbor Smart(bool smartImage);

        Thumbor Format(Thumbor.ImageFormat imageFormat);

        Thumbor Crop(int topLeft, int topRight, int bottomLeft, int bottomRight);

        Thumbor Quality(int? quality);

        Thumbor Grayscale(bool grayscaleImage);

        string ToUrl();
    }
}
