namespace DotNetThumbor
{
    public interface IThumbor
    {
        ThumborImage BuildImage(string imageUrl);

        string BuildUrl(string imageUrl);
    }
}
