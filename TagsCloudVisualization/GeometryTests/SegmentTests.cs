using System;
using FluentAssertions;
using Geometry;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    public class SegmentTests
    {
        [Test]
        public void TestFailsWhenPivotPointsCoincide()
        {
            Point a = new Point(1, 2);
            Point b = new Point(1, 2);

            Action act = () => new Segment(a, b);

            act.ShouldThrow<ArgumentException>();
        }

        public static TestCaseData[] ContainsCases =
        {
            new TestCaseData(new Segment(new Point(0, 0), new Point(1, 0)), new Point(0.5, 0)).Returns(true),
            new TestCaseData(new Segment(new Point(0, 0), new Point(1, 0)), new Point(1, 0)).Returns(true),
            new TestCaseData(new Segment(new Point(0, 0), new Point(1, 0)), new Point(2, 0)).Returns(false),
            new TestCaseData(new Segment(new Point(0, 0), new Point(1, 0)), new Point(2, 2)).Returns(false)   
        };
        [TestCaseSource(nameof(ContainsCases))]
        public bool TestContains(Segment segment, Point p)
        {
            return segment.Contains(p);
        }

        public static TestCaseData[] IntersectLineCases =
        {
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 1)),
                new Line(new Point(0, 1), new Point(1, 0)))
                .Returns(new Point(0.5, 0.5)),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Line(new Point(1, 0), new Point(1, 1)))
                .Returns(new Point(1, 0)),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Line(new Point(-1, 0), new Point(-1, 1)))
                .Returns(null),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Line(new Point(0, 0), new Point(1, 0)))
                .Returns(null),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Line(new Point(0, 1), new Point(1, 1)))
                .Returns(null), 
        };
        [TestCaseSource(nameof(IntersectLineCases))]
        public Point TestIntersectWithLine(Segment segment, Line line)
        {
            return segment.IntersectWith(line);
        }

        public static TestCaseData[] IntersectRayCases =
        {
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(0, 1), new Point(1, 0)))
                .Returns(new Point(0.5, 0.5)),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Ray(new Point(1, 0), new Point(1, 1)))
                .Returns(new Point(1, 0)),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Ray(new Point(-1, 0), new Point(-1, 1)))
                .Returns(null),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Ray(new Point(0.5, 1), new Point(0.5, 2)))
                .Returns(null),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Ray(new Point(0, 1), new Point(1, 1)))
                .Returns(null)
        };
        [TestCaseSource(nameof(IntersectRayCases))]
        public Point TestIntersectWithRay(Segment segment, Ray ray)
        {
            return segment.IntersectWith(ray);
        }

        public static TestCaseData[] IntersectSegmentCases =
        {
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 1)),
                new Segment(new Point(0, 1), new Point(1, 0)))
                .Returns(new Point(0.5, 0.5)),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(1, 0), new Point(1, 1)))
                .Returns(new Point(1, 0)),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(-1, 0), new Point(-1, 1)))
                .Returns(null),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(0.5, 1), new Point(0.5, 2)))
                .Returns(null),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(0.5, -2), new Point(0.5, -1)))
                .Returns(null),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(0, 1), new Point(1, 1)))
                .Returns(null),
        };
        [TestCaseSource(nameof(IntersectSegmentCases))]
        public Point TestIntersectWithSegment(Segment first, Segment second)
        {
            return first.IntersectWith(second);
        }

        public static TestCaseData[] DistanceCases =
        {
            new TestCaseData(new Segment(new Point(0, 0), new Point(2, 0)), new Point(1, 1), 1),
            new TestCaseData(new Segment(new Point(0, 0), new Point(2, 0)), new Point(3, 1), Math.Sqrt(2)),
            new TestCaseData(new Segment(new Point(0, 0), new Point(2, 0)), new Point(-1, 1), Math.Sqrt(2)),
            new TestCaseData(new Segment(new Point(0, 0), new Point(2, 0)), new Point(-1, 0), 1),
            new TestCaseData(new Segment(new Point(0, 0), new Point(2, 0)), new Point(1, 0), 0),
        };
        [TestCaseSource(nameof(DistanceCases))]
        public void TestDistance(Segment segment, Point p, double expected)
        {
            segment.DistanceTo(p).Should().BeApproximately(expected, DoubleComparer.DefaultEpsilon);
        }

        public static TestCaseData[] EqualsCases =
        {
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(0, 0), new Point(1, 0)))
                .Returns(true),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(1, 0), new Point(0, 0)))
                .Returns(true),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(2, 0), new Point(0, 0)))
                .Returns(false),
            new TestCaseData(
                new Segment(new Point(0, 0), new Point(1, 0)),
                new Segment(new Point(0, 0), new Point(0, 1)))
                .Returns(false),
        };
        [TestCaseSource(nameof(EqualsCases))]
        public bool TestEquals(Segment first, Segment second)
        {
            return first.Equals(second);
        }
    }
}
