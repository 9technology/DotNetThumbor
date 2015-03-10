namespace DotNetThumbor
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class ThumborSigner
    {

        // base64.urlsafe_b64encode(hmac.new("355b08da1ed57db2ec8014032f68ec1a"[:16], unicode("https://s3-ap-southeast-2.amazonaws.com/mi9public/image_resizer_test/original/text.jpg").encode('utf-8'), hashlib.sha1).digest())
        public string Encode(string input, byte[] key)
        {
            var hmacsha1 = new HMACSHA1(key);
            var byteArray = Encoding.UTF8.GetBytes(input);
            var stream = new MemoryStream(byteArray);
            var tmp = hmacsha1.ComputeHash(stream);

            return Convert.ToBase64String(tmp).Replace("+", "-").Replace("/", "_");
        }
    }
}
