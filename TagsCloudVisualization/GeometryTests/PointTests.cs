using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using NUnit.Framework;
using FluentAssertions;

// ReSharper disable InconsistentNaming

namespace GeometryTests
{
    [TestFixture]
    public class PointTests
    {
        [Test]
        public void TestEqualsPoints()
        {
            var A = new Point(0, 2);
            var B = new Point(-5e-10, 2 + 1e-10);
            A.Should().Be(B);
        }

        [Test]
        public void TestAddition()
        {
            var A = new Point(1, 2);
            var B = new Point(3, -1);
            (A + B).Should().Be(new Point(4, 1));
        }

        [Test]
        public void TestSubtraction()
        {
            var A = new Point(1, 2);
            var B = new Point(3, -1);
            (A - B).Should().Be(new Point(-2, 3));
        }

        [Test]
        public void TestLength()
        {
            var A = new Point(1, 2);
            A.Length.Should().BeApproximately(Math.Sqrt(5), 1e-9);
        }

        [Test]
        public void TestDotProduct()
        {
            var A = new Point(1, 2);
            var B = new Point(3, -1);
            A.DotProduct(B).Should().Be(1);
        }

        [Test]
        public void TestCrossProduct()
        {
            var A = new Point(1, 2);
            var B = new Point(3, -1);
            A.CrossProduct(B).Should().Be(-7);
        }

        [Test]
        public void TestMultiplying()
        {
            var A = new Point(3, -1);
            var k = -2;
            (A * k).Should().Be(new Point(-6, 2));
        }

        [Test]
        public void TestMultiplyingCommutativity()
        {
            var A = new Point(3, -1);
            var k = -2;
            (A * k).Should().Be(k * A);
        }

        [Test]
        public void TestDivision()
        {
            var A = new Point(3, -1);
            var k = -2;
            (A / k).Should().Be(new Point(-1.5, 0.5));
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

        public static TestCaseData[] TwoPointDirectionsCases =
        {
            new TestCaseData(new Point(0, 1), new Point(1, 1)).Returns(false),
            new TestCaseData(new Point(0, 1), new Point(-1, 0)).Returns(false),
            new TestCaseData(new Point(0, 1), new Point(0, 10)).Returns(true),
            new TestCaseData(new Point(1, 0), new Point(0, 0)).Returns(true),
            new TestCaseData(new Point(0, 0), new Point(0, 0)).Returns(true),
        };
        [TestCaseSource(nameof(TwoPointDirectionsCases))]
        public bool TestTwoPointDirections(Point first, Point second)
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
        public Point TestRotation(Point target, double angle)
        {
            return target.Rotate(angle);
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
            Point A = new Point(1, 2);
            Point B = new Point(4, 5);
            A.DistanceTo(B).Should().BeApproximately(3 * Math.Sqrt(2), DoubleComparer.DefaultEpsilon);
        }
    }
}
