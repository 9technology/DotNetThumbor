namespace DotNetThumborTest
{
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
            var thumborSigner = new ThumborSigner();
            var result = thumborSigner.Encode("http://input.jpg", "sample_key");

            result.Should().Be("2V__oFvPsslOdCY84FC7Sf6WeXI=");
        }

        [Test]
        public void TestEncodeSignerWithSlashReplacement()
        {
            var thumborSigner = new ThumborSigner();
            var result = thumborSigner.Encode("http://input.jpg", "ze_key"); // should produce / in hash

            result.Should().Be("3GUda_RJ29Oev5a4JMOysmQZmQA=");
        }

        [Test]
        public void TestEncodeSignerWithSingleCharacter()
        {
            var thumborSigner = new ThumborSigner();
            var result = thumborSigner.Encode("fit-in/200x200/http://example.org/input.jpg", "a");

            result.Should().Be("0X5nlVGRT9gn6PgbvaZyxQNbgKQ=");
        }
    }
}
