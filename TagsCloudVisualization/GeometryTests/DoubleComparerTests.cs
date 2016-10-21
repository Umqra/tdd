using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    public class DoubleComparerTests
    {
        [TestCase(0, 1, 0.1, ExpectedResult = false)]
        [TestCase(0, 1, 2, ExpectedResult = true)]
        [TestCase(0, 1e-10, DoubleComparer.DefaultEpsilon, ExpectedResult = true)]
        public bool TestEquals(double a, double b, double eps)
        {
            return a.EqualTo(b, eps);
        }

        [TestCase(0, 0.0001, DoubleComparer.DefaultEpsilon, ExpectedResult = true)]
        [TestCase(0, 1e-10, DoubleComparer.DefaultEpsilon, ExpectedResult = false)]
        [TestCase(2, 1.99999, DoubleComparer.DefaultEpsilon, ExpectedResult = false)]
        public bool TestLess(double a, double b, double eps)
        {
            return a.LessThan(b, eps);
        }

        [TestCase(0, 0.0001, DoubleComparer.DefaultEpsilon, ExpectedResult = true)]
        [TestCase(0, 1e-10, DoubleComparer.DefaultEpsilon, ExpectedResult = true)]
        [TestCase(2, 1.99999, DoubleComparer.DefaultEpsilon, ExpectedResult = false)]
        public bool TestLessThanOrEqualTo(double a, double b, double eps)
        {
            return a.LessThanOrEqualTo(b, eps);
        }

        [TestCase(0, 0.0001, DoubleComparer.DefaultEpsilon, ExpectedResult = false)]
        [TestCase(0, 1e-10, DoubleComparer.DefaultEpsilon, ExpectedResult = false)]
        [TestCase(2, 1.99999, DoubleComparer.DefaultEpsilon, ExpectedResult = true)]
        public bool TestGreater(double a, double b, double eps)
        {
            return a.GreaterThan(b, eps);
        }

        [TestCase(0, 0.0001, DoubleComparer.DefaultEpsilon, ExpectedResult = false)]
        [TestCase(0, 1e-10, DoubleComparer.DefaultEpsilon, ExpectedResult = true)]
        [TestCase(2, 1.99999, DoubleComparer.DefaultEpsilon, ExpectedResult = true)]
        public bool TestGreaterThanOrEqualTo(double a, double b, double eps)
        {
            return a.GreaterThanOrEqualTo(b, eps);
        }
    }
}
