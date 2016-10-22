using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Geometry;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace GeometryTests
{
    [TestFixture]
    public class LineTests
    {
        [Test]
        public void TestFailsWhenPivotPointsCoincide()
        {
            Point A = new Point(1, 2);
            Point B = new Point(1, 2);
            Action act = () => new Line(A, B);
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

        public static TestCaseData[] ContainCases =
        {
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(2, 0)).Returns(true),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 1)), new Point(3, 4)).Returns(false),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 1)), new Point(0, 0)).Returns(true),
        };
        [TestCaseSource(nameof(ContainCases))]
        public bool TestContain(Line targetLine, Point testedPoint)
        {
            return targetLine.ContainsPoint(testedPoint);
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
        public Point TestIntersectNonParallerlLines(Line first, Line second)
        {
            return first.IntersectWith(second);
        }

        [Test]
        public void TestIntersectCoincideLines()
        {
            Point A = new Point(0, 0);
            Point B = new Point(1, 0);
            Point C = new Point(-2, 0);
            Point D = new Point(4, 0);
            new Line(A, B).IntersectWith(new Line(C, D)).Should().BeNull();
        }

        [Test]
        public void TestIntersectParallelLines()
        {
            Point A = new Point(0, 0);
            Point B = new Point(1, 0);
            Point C = new Point(-2, 1);
            Point D = new Point(4, 1);
            new Line(A, B).IntersectWith(new Line(C, D)).Should().BeNull();
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
        public void TestDistance(Line target, Point checkedPoint, double expected)
        {
            target.DistanceTo(checkedPoint).Should().BeApproximately(expected, DoubleComparer.DefaultEpsilon);
        }

        public static TestCaseData[] PerpendicularCases =
        {
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(1, 1)).Returns(new Point(1, 0)),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 0)), new Point(2, 0)).Returns(new Point(2, 0)),
            new TestCaseData(new Line(new Point(0, 0), new Point(1, 1)), new Point(0, 1)).Returns(new Point(0.5, 0.5))
        };
        [TestCaseSource(nameof(PerpendicularCases))]
        public Point TestPerpendicular(Line target, Point from)
        {
            return target.PerpendicularFrom(from);
        }
    }
}
