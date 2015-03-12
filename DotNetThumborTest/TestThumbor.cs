namespace DotNetThumborTest
{
    using System;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestThumbor
    {
        [Test]
        [ExpectedException(typeof(UriFormatException))]
        [TestCase("")]
        [TestCase("notaurl")]
        [TestCase("httpnoturl")]
        public void TestThumborUrlIsARealUrl(string url)
        {
            var thumbor = new Thumbor(url);
            thumbor.Should().BeNull("This should never run");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        [TestCase("")]
        [TestCase("notaurl")]
        [TestCase("httpnoturl")]
        public void BuildImageWithInvalidUrl(string url)
        {
            var thumbor = new Thumbor("http://localhost/");
            thumbor.BuildImage(url);
        }

        [Test]
        [TestCase("/trim/100x200/filters:grayscale()/http://myserver/myimage.jpg")]
        public void BuildUrlWithoutSecretKey(string url)
        {
            var thumbor = new Thumbor("http://localhost/");
            var thumborUrl = thumbor.BuildSignedUrl(url);
            thumborUrl.Should().Be("http://localhost/unsafe/trim/100x200/filters:grayscale()/http://myserver/myimage.jpg");
        }

        [Test]
        [TestCase("trim/100x200/filters:grayscale()/http://myserver/myimage.jpg")]
        public void BuildUrlWithSecretKey(string url)
        {
            var thumbor = new Thumbor("http://localhost/", "secret_key");
            var thumborUrl = thumbor.BuildSignedUrl(url);
            thumborUrl.Should().Be("/BBkKn1mqVJVyKDd3PFh58ATT-dQ=/trim/100x200/filters:grayscale()/http://myserver/myimage.jpg");
        }

        // The following tests come from the Thumbor Library Test Scenarios
        /*
        Given
            A security key of 'my-security-key'
            And an image URL of "my.server.com/some/path/to/image.jpg"
            And a width of 300
            And a height of 200
        When
            I ask my library for a signed url
        Then
            I get '/8ammJH8D-7tXy6kU3lTvoXlhu4o=/300x200/my.server.com/some/path/to/image.jpg' as url
         */
        [Test]
        public void TestScenario1()
        {
            var thumbor = new Thumbor("http://localhost/", "my-security-key");
            var thumborUrl = thumbor.BuildSignedUrl("300x200/my.server.com/some/path/to/image.jpg");
            thumborUrl.Should().Be("/8ammJH8D-7tXy6kU3lTvoXlhu4o=/300x200/my.server.com/some/path/to/image.jpg");
        }

        /*
        Given
            A security key of 'my-security-key'
            And an image URL of "my.server.com/some/path/to/image.jpg"
            And a width of 300
            And a height of 200
        When
            I ask my library for an encrypted URL
        Then
            I get the proper url (/8ammJH8D-7tXy6kU3lTvoXlhu4o=/300x200/my.server.com/some/path/to/image.jpg)
         */
        [Test]
        public void TestScenario2()
        {
            var thumbor = new Thumbor("http://localhost/", "my-security-key");
            var thumborUrl = thumbor.BuildEncryptedUrl("300x200/my.server.com/some/path/to/image.jpg");
            thumborUrl.Should().Be("/8ammJH8D-7tXy6kU3lTvoXlhu4o=/300x200/my.server.com/some/path/to/image.jpg");
        }

        /*
        Given
            A security key of 'my-security-key'
            And an image URL of "my.server.com/some/path/to/image.jpg"
            And the meta flag
        When
            I ask my library for an encrypted URL
        Then
            I get the proper url (/Ps3ORJDqxlSQ8y00T29GdNAh2CY=/meta/my.server.com/some/path/to/image.jpg)
         */
        [Test]
        public void TestScenario3()
        {
            var thumbor = new Thumbor("http://localhost/", "my-security-key");
            var thumborUrl = thumbor.BuildEncryptedUrl("meta/my.server.com/some/path/to/image.jpg");
            thumborUrl.Should().Be("/Ps3ORJDqxlSQ8y00T29GdNAh2CY=/meta/my.server.com/some/path/to/image.jpg");
        }

        /*
        Given
            A security key of 'my-security-key'
            And an image URL of "my.server.com/some/path/to/image.jpg"
            And the smart flag
        When
            I ask my library for an encrypted URL
        Then
            I get the proper url (/-2NHpejRK2CyPAm61FigfQgJBxw=/smart/my.server.com/some/path/to/image.jpg)
         */
        [Test]
        public void TestScenario4()
        {
            var thumbor = new Thumbor("http://localhost/", "my-security-key");
            var thumborUrl = thumbor.BuildEncryptedUrl("smart/my.server.com/some/path/to/image.jpg");
            thumborUrl.Should().Be("/-2NHpejRK2CyPAm61FigfQgJBxw=/smart/my.server.com/some/path/to/image.jpg");
        }

        /*
        Given
            A security key of 'my-security-key'
            And an image URL of "my.server.com/some/path/to/image.jpg"
            And the fit-in flag
        When
            I ask my library for an encrypted URL
        Then
            I get the proper url (/uvLnA6TJlF-Cc-L8z9pEtfasO3s=/fit-in/my.server.com/some/path/to/image.jpg)
         */
        [Test]
        public void TestScenario5()
        {
            var thumbor = new Thumbor("http://localhost/", "my-security-key");
            var thumborUrl = thumbor.BuildEncryptedUrl("fit-in/my.server.com/some/path/to/image.jpg");
            thumborUrl.Should().Be("/uvLnA6TJlF-Cc-L8z9pEtfasO3s=/fit-in/my.server.com/some/path/to/image.jpg");
        }

        /*
        Given
            A security key of 'my-security-key'
            And an image URL of "my.server.com/some/path/to/image.jpg"
            And a 'quality(20)' filter
            And a 'brightness(10)' filter
        When
            I ask my library for an encrypted URL
        Then
            I get the proper url (/ZZtPCw-BLYN1g42Kh8xTcRs0Qls=/filters:brightness(10):contrast(20)/my.server.com/some/path/to/image.jpg)
         */
        [Test]
        public void TestScenario6()
        {
            var thumbor = new Thumbor("http://localhost/", "my-security-key");
            var thumborUrl = thumbor.BuildEncryptedUrl("filters:brightness(10):contrast(20)/my.server.com/some/path/to/image.jpg");
            thumborUrl.Should().Be("/ZZtPCw-BLYN1g42Kh8xTcRs0Qls=/filters:brightness(10):contrast(20)/my.server.com/some/path/to/image.jpg");
        }
    }
}
