﻿using System;
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
    public class RayTests
    {
        [Test]
        public void TestFailsWhenPivotPointsCoincide()
        {
            Point A = new Point(1, 2);
            Point B = new Point(1, 2);
            Action act = () => new Ray(A, B);
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
        public bool TestContains(Ray ray, Point P)
        {
            return ray.Contains(P);
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
        public Point TestIntersectionWithLine(Ray ray, Line line)
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
        public Point TestIntersectionWithRay(Ray first, Ray second)
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
        public void TestDistance(Ray targetRay, Point checkedPoint, double expected)
        {
            targetRay.DistanceTo(checkedPoint).Should().BeApproximately(expected, DoubleComparer.DefaultEpsilon);
        }
    }
}
