namespace DotNetThumbor
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class ThumborSigner : IThumborSigner
    {
        /// <summary>
        /// Method to sign Thumbor urls correct as of 2015/03/12
        /// </summary>
        /// <param name="input">The image URL that thumbor expects</param>
        /// <param name="key">The thumbor secret key</param>
        /// <returns>The signed result which can be passed to thumbor</returns>
        public string Encode(string input, string key)
        {
            var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var byteArray = Encoding.UTF8.GetBytes(input);
            var stream = new MemoryStream(byteArray);
            var tmp = hmacsha1.ComputeHash(stream);

            // Thumbor implementation replaces + and / and so is replicated here
            return Convert.ToBase64String(tmp).Replace("+", "-").Replace("/", "_");
        }
    }
}
