using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public static class DoubleComparer
    {
        public const double DefaultEpsilon = 1e-9;

        public static bool EqualTo(this double a, double b, double eps = DefaultEpsilon)
        {
            return Math.Abs(a - b) < eps;
        }

        public static bool LessThanOrEqualTo(this double a, double b, double eps = DefaultEpsilon)
        {
            return a < b || a.EqualTo(b, eps);
        }

        public static bool LessThan(this double a, double b, double eps = DefaultEpsilon)
        {
            return a < b && !a.EqualTo(b, eps);
        }

        public static bool GreaterThanOrEqualTo(this double a, double b, double eps = DefaultEpsilon)
        {
            return a < b || a.EqualTo(b, eps);
        }

        public static bool GreaterThan(this double a, double b, double eps = DefaultEpsilon)
        {
            return a > b && !a.EqualTo(b, eps);
        }
    }
}
