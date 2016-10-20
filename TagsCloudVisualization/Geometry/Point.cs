using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Point
    {
        // ReSharper disable InconsistentNaming
        public readonly double x;
        public readonly double y;
        // ReSharper restore InconsistentNaming

        public double Length => Math.Sqrt(x * x + y * y);

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static Point operator + (Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }

        public static Point operator *(Point a, double k)
        {
            return new Point(a.x * k, a.y * k);
        }

        public static Point operator *(double k, Point a)
        {
            return new Point(a.x * k, a.y * k);
        }

        public static Point operator /(Point a, double k)
        {
            return new Point(a.x / k, a.y / k);
        }

        public double DotProduct(Point other)
        {
            return x * other.x + y * other.y;
        }

        public double CrossProduct(Point other)
        {
            return x * other.y - y * other.x;
        }

        protected bool Equals(Point other)
        {
            return x.EqualTo(other.x) && y.EqualTo(other.y);
        }

#pragma warning disable 659
        // Can't generate .GetHashCode() because Equals complicated and non-transitive
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point)obj);
        }
    }
}
