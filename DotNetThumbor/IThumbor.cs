namespace DotNetThumbor
{
    public interface IThumbor
    {
        ThumborImage BuildImage(string imageUrl);

        string BuildSignedUrl(string imageUrl);

        string BuildEncryptedUrl(string imageUrl);
    }
}
