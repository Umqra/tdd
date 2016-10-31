using System;
using FluentAssertions;
using Geometry;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    public class LineTests
    {
        [Test]
        public void TestFailsWhenPivotPointsCoincide()
        {
            Point a = new Point(1, 2);
            Point b = new Point(1, 2);

            Action act = () => new Line(a, b);

            act.ShouldThrow<ArgumentException>();
        }

        public static TestCaseData[] ParallelCases =
        {
            new TestCaseData(
                new Line(new Point(1, 1), new Point(2, 2)), 
                new Line(new Point(-1, -1), new Point(-3, -3)))
                .Returns(true), 
            new TestCaseData(
                new Line(new Point(0, 0), new Point(1, 0)),
                new Line(new Point(0, 0), new Point(0, 1)))
                .Returns(false), 
        };
        [TestCaseSource(nameof(ParallelCases))]
        public bool TestParallel(Line first, Line second)
        {
            return first.ParallelTo(second);
        }

        public static TestCaseData[] ContainsCases =
        {
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(2, 0)).Returns(true),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 1)), new Point(3, 4)).Returns(false),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 1)), new Point(0, 0)).Returns(true),
        };
        [TestCaseSource(nameof(ContainsCases))]
        public bool TestContains(Line line, Point p)
        {
            return line.Contains(p);
        }

        public static TestCaseData[] NonParallelIntersectionCases =
        {
            new TestCaseData(
                new Line(new Point(0, 0), new Point(1, 1)), 
                new Line(new Point(0, 1), new Point(1, 0)))
                .Returns(new Point(0.5, 0.5)),
            new TestCaseData(
                new Line(new Point(0, 0), new Point(4, 1)), 
                new Line(new Point(1, 0), new Point(1, 1)))
                .Returns(new Point(1, 0.25)),
            new TestCaseData(
                new Line(new Point(0, 0), new Point(4, 2)),
                new Line(new Point(1, 0), new Point(3, 2)))
                .Returns(new Point(2, 1)), 
        };
        [TestCaseSource(nameof(NonParallelIntersectionCases))]
        public Point? TestIntersectNonParallerlLines(Line first, Line second)
        {
            return first.IntersectWith(second);
        }

        [Test]
        public void TestIntersectCoincideLines()
        {
            Point a = new Point(0, 0);
            Point b = new Point(1, 0);
            Point c = new Point(-2, 0);
            Point d = new Point(4, 0);
            
            new Line(a, b).IntersectWith(new Line(c, d)).Should().BeNull();
        }

        [Test]
        public void TestIntersectParallelLines()
        {
            Point a = new Point(0, 0);
            Point b = new Point(1, 0);
            Point c = new Point(-2, 1);
            Point d = new Point(4, 1);

            new Line(a, b).IntersectWith(new Line(c, d)).Should().BeNull();
        }

        public static TestCaseData[] EqualsCases =
        {
            new TestCaseData(
                new Line(new Point(0, 0), new Point(1, 1)), 
                new Line(new Point(2, 2), new Point(3, 3)))
                .Returns(true),
            new TestCaseData(
                new Line(new Point(1, 1), new Point(2, 2)), 
                new Line(new Point(1, 0), new Point(2, 1)))
                .Returns(false)
        };
        [TestCaseSource(nameof(EqualsCases))]
        public bool TestEquals(Line first, Line second)
        {
            return first.Equals(second);
        }

        public static TestCaseData[] DistanceCases =
        {
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(1, 1), 1),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(10, 0), 0),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 1)), new Point(0, 1), Math.Sqrt(2) / 2),
        };
        [TestCaseSource(nameof(DistanceCases))]
        public void TestDistance(Line line, Point p, double expected)
        {
            line.DistanceTo(p).Should().BeApproximately(expected, DoubleComparer.DefaultEpsilon);
        }

        public static TestCaseData[] PerpendicularCases =
        {
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(1, 1)).Returns(new Point(1, 0)),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(2, 0)).Returns(new Point(2, 0)),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 1)), new Point(0, 1)).Returns(new Point(0.5, 0.5))
        };
        [TestCaseSource(nameof(PerpendicularCases))]
        public Point TestPerpendicular(Line line, Point p)
        {
            return line.PerpendicularFrom(p);
        }
    }
}
