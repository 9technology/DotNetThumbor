namespace DotNetThumbor
{
    public interface IThumborSigner
    {
        string Encode(string input, string key);
    }
}
