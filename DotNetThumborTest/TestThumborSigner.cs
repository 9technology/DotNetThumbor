namespace DotNetThumborTest
{
    using System.Text;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestThumborSigner
    {
        // Verified against https://quickhash.com/ for hmac sha1 which is used by thumbor
        [Test]
        public void TestEncodeSigner()
        {
            var key = Encoding.UTF8.GetBytes("sample_key");
            var result = ThumborSigner.Encode("http://input.jpg", key);

            result.Should().Be("2V__oFvPsslOdCY84FC7Sf6WeXI=");
        }

        [Test]
        public void TestEncodeSignerWithSlashReplacement()
        {
            var key = Encoding.UTF8.GetBytes("ze_key");
            var result = ThumborSigner.Encode("http://input.jpg", key); // should produce / in hash

            result.Should().Be("3GUda_RJ29Oev5a4JMOysmQZmQA=");
        }
    }
}
