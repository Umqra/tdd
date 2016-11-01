using System;
using FluentAssertions;
using Geometry;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    public class RayTests
    {
        [Test]
        public void TestFailsWhenPivotPointsCoincide()
        {
            Point a = new Point(1, 2);
            Point b = new Point(1, 2);

            Action act = () => new Ray(a, b);

            act.ShouldThrow<ArgumentException>();
        }

        public static TestCaseData[] ContainsCases =
        {
            new TestCaseData(new Ray(new Point(0, 0), new Point(1, 1)), new Point(2, 2)).Returns(true),
            new TestCaseData(new Ray(new Point(0, 0), new Point(1, 1)), new Point(0, 0)).Returns(true),
            new TestCaseData(new Ray(new Point(0, 0), new Point(1, 1)), new Point(-1, -1)).Returns(false),
            new TestCaseData(new Ray(new Point(0, 0), new Point(1, 1)), new Point(0, 1)).Returns(false),
        };
        [TestCaseSource(nameof(ContainsCases))]
        public bool TestContains(Ray ray, Point p)
        {
            return ray.Contains(p);
        }

        public static TestCaseData[] IntersectLineCases =
        {
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)), 
                new Line(new Point(1, 0), new Point(0, 1)))
                .Returns(new Point(0.5, 0.5)), 
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Line(new Point(-1, 1), new Point(1, -1)))
                .Returns(new Point(0, 0)),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Line(new Point(-1, 0), new Point(-1, 1)))
                .Returns(null),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Line(new Point(0, 0), new Point(1, 1)))
                .Returns(null),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Line(new Point(1, 0), new Point(2, 1)))
                .Returns(null)
        };

        [TestCaseSource(nameof(IntersectLineCases))]
        public Point? TestIntersectionWithLine(Ray ray, Line line)
        {
            return ray.IntersectWith(line);
        }

        public static TestCaseData[] IntersectRayCases =
        {
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)), 
                new Ray(new Point(1, 0), new Point(0, 1))) 
                .Returns(new Point(0.5, 0.5)),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(0, 0), new Point(-1, 1)))
                .Returns(new Point(0, 0)),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(-1, 0), new Point(-1, 1)))
                .Returns(null),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(0, 1), new Point(0, 2)))
                .Returns(null),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(0, 0), new Point(1, 1)))
                .Returns(null),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(0, 1), new Point(1, 2)))
                .Returns(null)
        };
        [TestCaseSource(nameof(IntersectRayCases))]
        public Point? TestIntersectionWithRay(Ray first, Ray second)
        {
            return first.IntersectWith(second);
        }

        public static TestCaseData[] EqualsCases =
        {
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(0, 0), new Point(3, 3)))
                .Returns(true),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 1)),
                new Ray(new Point(1, 1), new Point(0, 0)))
                .Returns(false),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(1, 0)),
                new Ray(new Point(0, 1), new Point(0, 0)))
                .Returns(false),
            new TestCaseData(
                new Ray(new Point(0, 0), new Point(2, 2)),
                new Ray(new Point(1, 1), new Point(2, 2)))
                .Returns(false),  
        };
        [TestCaseSource(nameof(EqualsCases))]
        public bool TestEquals(Ray first, Ray second)
        {
            return first.Equals(second);
        }

        public static TestCaseData[] DistanceCases =
        {
            new TestCaseData(new Ray(new Point(0, 0), new Point(1, 0)), new Point(1, 1), 1),
            new TestCaseData(new Ray(new Point(0, 0), new Point(1, 1)), new Point(0, 1), Math.Sqrt(2) / 2),
            new TestCaseData(new Ray(new Point(0, 0), new Point(1, 1)), new Point(-1, 0), 1)
        };
        [TestCaseSource(nameof(DistanceCases))]
        public void TestDistance(Ray ray, Point p, double expected)
        {
            ray.DistanceTo(p).Should().BeApproximately(expected, DoubleComparer.DefaultEpsilon);
        }
    }
}
