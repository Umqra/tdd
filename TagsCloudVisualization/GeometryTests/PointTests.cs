using System;
using Geometry;
using NUnit.Framework;
using FluentAssertions;

// CR: Nope, fix names
//! ReSharper disable InconsistentNaming

namespace GeometryTests
{
    [TestFixture]
    public class PointTests
    {
        [Test]
        public void TestEqualsPoints()
        {
            var a = new Point(0, 2);
            var b = new Point(-5e-10, 2 + 1e-10);
            a.Should().Be(b);
        }

        [Test]
        public void TestAddition()
        {
            var a = new Point(1, 2);
            var b = new Point(3, -1);
            (a + b).Should().Be(new Point(4, 1));
        }

        [Test]
        public void TestSubtraction()
        {
            var a = new Point(1, 2);
            var b = new Point(3, -1);
            (a - b).Should().Be(new Point(-2, 3));
        }

        [Test]
        public void TestLength()
        {
            var a = new Point(1, 2);
            a.Length.Should().BeApproximately(Math.Sqrt(5), 1e-9);
        }

        [Test]
        public void TestDotProduct()
        {
            var a = new Point(1, 2);
            var b = new Point(3, -1);
            a.DotProduct(b).Should().Be(1);
        }

        [Test]
        public void TestCrossProduct()
        {
            var a = new Point(1, 2);
            var b = new Point(3, -1);
            a.CrossProduct(b).Should().Be(-7);
        }

        [Test]
        public void TestMultiplying()
        {
            var a = new Point(3, -1);
            var k = -2;
            (a * k).Should().Be(new Point(-6, 2));
        }

        [Test]
        public void TestMultiplyingCommutativity()
        {
            var a = new Point(3, -1);
            var k = -2;
            (a * k).Should().Be(k * a);
        }

        [Test]
        public void TestDivision()
        {
            var a = new Point(3, -1);
            var k = -2;
            (a / k).Should().Be(new Point(-1.5, 0.5));
        }

        public static TestCaseData[] CollinearCases =
        {
            new TestCaseData(new Point(0, 0), new Point(1, 1)).Returns(true),
            new TestCaseData(new Point(1, -1), new Point(-2, 2)).Returns(true),
            new TestCaseData(new Point(1, -1), new Point(1, 1)).Returns(false),
        };
        [TestCaseSource(nameof(CollinearCases))]
        public bool TestCollinear(Point first, Point second)
        {
            return first.CollinearTo(second);
        }

        public static TestCaseData[] DirectionCases =
        {
            new TestCaseData(new Point(0, 1), new Point(1, 1)).Returns(false),
            new TestCaseData(new Point(0, 1), new Point(-1, 0)).Returns(false),
            new TestCaseData(new Point(0, 1), new Point(0, 10)).Returns(true),
            new TestCaseData(new Point(1, 0), new Point(0, 0)).Returns(true),
            new TestCaseData(new Point(0, 0), new Point(0, 0)).Returns(true),
        };
        [TestCaseSource(nameof(DirectionCases))]
        public bool TestDirection(Point first, Point second)
        {
            return first.HasSameDirectionAs(second);
        }


        public static TestCaseData[] RotationCases =
        {
            new TestCaseData(new Point(1, 0), Math.PI).Returns(new Point(-1, 0)),
            new TestCaseData(new Point(1, 0), Math.PI / 2).Returns(new Point(0, 1)),
            new TestCaseData(new Point(1, 1), -Math.PI / 2).Returns(new Point(1, -1)),
            new TestCaseData(new Point(1, 0), 1).Returns(new Point(Math.Cos(1), Math.Sin(1)))   
        };
        [TestCaseSource(nameof(RotationCases))]
        public Point TestRotation(Point direction, double angle)
        {
            return direction.Rotate(angle);
        }

        [Test]
        public void TestOrthogonal()
        {
            Point v = new Point(1, 2);
            v.Orthogonal.Should().Be(new Point(-2, 1));
        }

        [Test]
        public void TestDistanceTo()
        {
            Point a = new Point(1, 2);
            Point b = new Point(4, 5);
            a.DistanceTo(b).Should().BeApproximately(3 * Math.Sqrt(2), DoubleComparer.DefaultEpsilon);
        }


        public static TestCaseData[] QuarterCases =
        {
            new TestCaseData(new Point(0, 0)).Returns(0),
            new TestCaseData(new Point(1, 0)).Returns(1),
            new TestCaseData(new Point(0, 1)).Returns(2),
            new TestCaseData(new Point(-1, 0)).Returns(3),
            new TestCaseData(new Point(0, -1)).Returns(4),

            new TestCaseData(new Point(1, 1)).Returns(1),
            new TestCaseData(new Point(-1, 1)).Returns(2),
            new TestCaseData(new Point(-1, -1)).Returns(3),
            new TestCaseData(new Point(1, -1)).Returns(4),
        };
        [TestCaseSource(nameof(QuarterCases))]
        public int TestQuarter(Point p)
        {
            return p.Quater;
        }

        public static TestCaseData[] AngleCases =
        {
            new TestCaseData(new Point(1, 1), new Point(1, 1), 0),
            new TestCaseData(new Point(1, 1), new Point(-1, -1), Math.PI),
            new TestCaseData(new Point(1, 0), new Point(0, 1), Math.PI / 2),
            new TestCaseData(new Point(0, 1), new Point(1, 0), -Math.PI / 2),
            new TestCaseData(new Point(0, 0), new Point(1, 1), 0),
            new TestCaseData(new Point(1, 1), new Point(0, 0), 0),
            new TestCaseData(new Point(0, 0), new Point(0, 0), 0),
        };
        [TestCaseSource(nameof(AngleCases))]
        public void TestAngleTo(Point from, Point to, double expected)
        {
            from.AngleTo(to).Should().BeApproximately(expected, DoubleComparer.DefaultEpsilon);
        }
    }
}
