namespace DotNetThumbor
{
    public interface IThumbor
    {
        void BuildImage(string imageUrl);

        string ToUrl();
    }
}
