namespace DotNetThumbor
{
    public interface IThumbor
    {
        Thumbor BuildImage(string imageUrl);

        Thumbor Resize(int? width, int? height);

        Thumbor Smart(bool beSmart);

        Thumbor Format(Thumbor.ImageFormat imageFormat);

        Thumbor Crop(int topLeft, int topRight, int bottomLeft, int bottomRight);

        string ToUrl();
    }
}
