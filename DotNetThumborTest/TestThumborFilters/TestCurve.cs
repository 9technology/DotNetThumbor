namespace DotNetThumborTest.TestThumborFilters
{
    using System;
    using System.Collections.Generic;

    using DotNetThumbor;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class TestCurve
    {
        [Test]
        public void ThumborCurveFilter()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl =
                thumbor.BuildImage("http://localhost/image.jpg")
                       .Curve(
                        new List<Tuple<int, int>> { Tuple.Create(1, 2) },
                        new List<Tuple<int, int>> { Tuple.Create(3, 4) },
                        new List<Tuple<int, int>> { Tuple.Create(5, 6) },
                        new List<Tuple<int, int>> { Tuple.Create(7, 8) })
                       .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:curve([(1,2)],[(3,4)],[(5,6)],[(7,8)])/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborWithEmptyCurveFilter()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl =
                thumbor.BuildImage("http://localhost/image.jpg")
                       .Curve(
                        new List<Tuple<int, int>>(),
                        new List<Tuple<int, int>> { Tuple.Create(3, 4) },
                        new List<Tuple<int, int>> { Tuple.Create(5, 6) },
                        new List<Tuple<int, int>> { Tuple.Create(7, 8) })
                       .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:curve([],[(3,4)],[(5,6)],[(7,8)])/http://localhost/image.jpg");
        }

        [Test]
        public void ThumborCurveFilterMutiTuple()
        {
            var thumbor = new Thumbor("http://localhost/");
            var resizedUrl =
                thumbor.BuildImage("http://localhost/image.jpg")
                       .Curve(
                        new List<Tuple<int, int>> { Tuple.Create(1, 2), Tuple.Create(11, 12) },
                        new List<Tuple<int, int>> { Tuple.Create(3, 4), Tuple.Create(13, 14) },
                        new List<Tuple<int, int>> { Tuple.Create(5, 6), Tuple.Create(15, 16) },
                        new List<Tuple<int, int>> { Tuple.Create(7, 8), Tuple.Create(17, 18) })
                       .ToFullUrl();
            resizedUrl.Should().Be("http://localhost/unsafe/filters:curve([(1,2),(11,12)],[(3,4),(13,14)],[(5,6),(15,16)],[(7,8),(17,18)])/http://localhost/image.jpg");
        }
    }
}
