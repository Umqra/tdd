using System;

namespace Geometry
{
    public static class DoubleComparer
    {
        public const double DefaultEpsilon = 1e-9;

        public static bool ApproxEqualTo(this double a, double b, double eps = DefaultEpsilon)
        {
            return Math.Abs(a - b) < eps;
        }

        public static bool ApproxLessOrEqualTo(this double a, double b, double eps = DefaultEpsilon)
        {
            return a < b || a.ApproxEqualTo(b, eps);
        }

        public static bool ApproxLess(this double a, double b, double eps = DefaultEpsilon)
        {
            return a < b && !a.ApproxEqualTo(b, eps);
        }

        public static bool ApproxGreaterOrEqualTo(this double a, double b, double eps = DefaultEpsilon)
        {
            return a > b || a.ApproxEqualTo(b, eps);
        }

        public static bool ApproxGreater(this double a, double b, double eps = DefaultEpsilon)
        {
            return a > b && !a.ApproxEqualTo(b, eps);
        }
    }
}
