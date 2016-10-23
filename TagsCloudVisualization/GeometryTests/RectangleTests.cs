using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Geometry;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    public class RectangleTests
    {
        [Test]
        public void TestConstructFromTwoCorners()
        {
            var rectangle = new Rectangle(new Point(0, 0), new Point(1, 1));
            rectangle.Should().Be(new Rectangle(new Point(0, 1), new Point(1, 0)));
        }

        [Test]
        public void TestSides()
        {
            var rectangle = new Rectangle(new Point(0, 1), new Point(1, 0));
            var expectedSides = new[]
            {
                new Segment(new Point(0, 0), new Point(0, 1)),
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(1, 1), new Point(1, 0)),
                new Segment(new Point(1, 1), new Point(0, 1))
            };
            rectangle.Sides.Should().OnlyContain(segment => expectedSides.Contains(segment));
        }

        public static TestCaseData[] IntersectCases =
        {
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(3, 3)),
                new Rectangle(new Point(1, 1), new Point(2, 2)))
                .Returns(new Rectangle(new Point(1, 1), new Point(2, 2))),
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(3, 1)),
                new Rectangle(new Point(1, -1), new Point(2, 2)))
                .Returns(new Rectangle(new Point(1, 0), new Point(2, 1))),
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(1, 2)),
                new Rectangle(new Point(1, 1), new Point(2, 3)))
                .Returns(new Rectangle(new Point(1, 1), new Point(1, 2))),
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(1, 1)),
                new Rectangle(new Point(1, 1), new Point(2, 2)))
                .Returns(new Rectangle(new Point(1, 1), new Point(1, 1))), 
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(1, 1)),
                new Rectangle(new Point(2, 2), new Point(3, 3)))
                .Returns(null) 
        };
        [TestCaseSource(nameof(IntersectCases))]
        public Rectangle TestIntersect(Rectangle first, Rectangle second)
        {
            return first.IntersectWith(second);
        }

        public static TestCaseData[] ContainsCases =
        {
            new TestCaseData(new Rectangle(new Point(0, 0), new Point(2, 2)), new Point(1, 1)).Returns(true),
            new TestCaseData(new Rectangle(new Point(0, 0), new Point(2, 2)), new Point(1, 2)).Returns(true),
            new TestCaseData(new Rectangle(new Point(0, 0), new Point(2, 2)), new Point(-1, 1)).Returns(false),
            new TestCaseData(new Rectangle(new Point(0, 0), new Point(2, 2)), new Point(-1, -1)).Returns(false),
        };
        [TestCaseSource(nameof(ContainsCases))]
        public bool Contains(Rectangle rectangle, Point P)
        {
            return rectangle.Contains(P);
        }

        public static TestCaseData[] TouchesCases =
        {
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(2, 2)), 
                new Rectangle(new Point(2, 2), new Point(3, 3)))
                .Returns(true), 
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(2, 2)),
                new Rectangle(new Point(2, 0), new Point(4, 2)))
                .Returns(true),
            new TestCaseData(
                new Rectangle(new Point(0, 0), new Point(2, 2)),
                new Rectangle(new Point(3, 0), new Point(5, 2)))
                .Returns(false)
        };
        [TestCaseSource(nameof(TouchesCases))]
        public bool TestTouches(Rectangle first, Rectangle second)
        {
            return first.Touches(second);
        }
    }

}
