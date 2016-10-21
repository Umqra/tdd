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
    public class SegmentTests
    {
        [Test]
        public void TestFailsWhenPivotPointsCoincide()
        {
            Point A = new Point(1, 2);
            Point B = new Point(1, 2);
            Action act = () => new Segment(A, B);
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
        public bool TestContains(Segment targetSegment, Point checkedPoint)
        {
            return targetSegment.ContainsPoint(checkedPoint);
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
        public Point TestIntersectWithLine(Segment targetSegment, Line testedLine)
        {
            return targetSegment.IntersectsWith(testedLine);
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
                .Returns(null),
        };
        [TestCaseSource(nameof(IntersectRayCases))]
        public Point TestIntersectWithRay(Segment targetSegment, Ray testedRay)
        {
            return targetSegment.IntersectsWith(testedRay);
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
            return first.IntersectsWith(second);
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
        public void TestDistance(Segment targetSegment, Point checkedPoint, double expected)
        {
            targetSegment.DistanceTo(checkedPoint).Should().BeApproximately(expected, DoubleComparer.DefaultEpsilon);
        }
    }
}
