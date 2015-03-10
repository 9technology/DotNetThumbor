namespace DotNetThumbor
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class ThumborSigner
    {
        public static string Encode(string input, byte[] key)
        {
            var hmacsha1 = new HMACSHA1(key);
            var byteArray = Encoding.UTF8.GetBytes(input);
            var stream = new MemoryStream(byteArray);
            var tmp = hmacsha1.ComputeHash(stream);

            return Convert.ToBase64String(tmp).Replace("+", "-").Replace("/", "_");
        }
    }
}
