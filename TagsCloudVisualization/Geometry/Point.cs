using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
#pragma warning disable 659

namespace Geometry
{
    public class Point
    {
        // ReSharper disable InconsistentNaming
        public readonly double x;
        public readonly double y;
        // ReSharper restore InconsistentNaming

        public Point Orthogonal => new Point(-y, x);
        public double Length => Math.Sqrt(x * x + y * y);

        public int Quater
        {
            get
            {
                if (this.Equals(new Point(0, 0)))
                    return 0;
                if (x > 0 && y >= 0)
                    return 1;
                if (x <= 0 && y > 0)
                    return 2;
                if (x < 0 && y <= 0)
                    return 3;
                if (x >= 0 && y < 0)
                    return 4;
                throw new ArgumentException();
            }
        }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static Point operator + (Point A, Point B)
        {
            return new Point(A.x + B.x, A.y + B.y);
        }

        public static Point operator -(Point A, Point B)
        {
            return new Point(A.x - B.x, A.y - B.y);
        }

        public static Point operator *(Point A, double k)
        {
            return new Point(A.x * k, A.y * k);
        }

        public static Point operator *(double k, Point A)
        {
            return new Point(A.x * k, A.y * k);
        }

        public static Point operator /(Point A, double k)
        {
            return new Point(A.x / k, A.y / k);
        }

        public double DotProduct(Point other)
        {
            return x * other.x + y * other.y;
        }

        public double CrossProduct(Point other)
        {
            return x * other.y - y * other.x;
        }

        public bool CollinearTo(Point other)
        {
            return this.CrossProduct(other).EqualTo(0);
        }

        public bool HasSameDirectionAs(Point other)
        {
            return this.CollinearTo(other) && this.DotProduct(other).GreaterThanOrEqualTo(0);
        }

        public Point Rotate(double angleInRadians)
        {
            double cosAngle = Math.Cos(angleInRadians);
            double sinAngle = Math.Sin(angleInRadians);
            return new Point(x * cosAngle - y * sinAngle, x * sinAngle + y * cosAngle);
        }

        public double DistanceTo(Point other)
        {
            return (this - other).Length;
        }

        protected bool Equals(Point other)
        {
            return x.EqualTo(other.x) && y.EqualTo(other.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point)obj);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
        
        public static explicit operator System.Drawing.PointF(Point P)
        {
            return new System.Drawing.PointF((float)P.x, (float)P.y);
        }

        public static explicit operator System.Drawing.Point(Point P)
        {
            return new System.Drawing.Point((int)Math.Round(P.x), (int)Math.Round(P.y));
        }

        public static explicit operator Point(System.Drawing.PointF P)
        {
            return new Point(P.X, P.Y);
        }

        public static explicit operator Point(System.Drawing.Point P)
        {
            return new Point(P.X, P.Y);
        }

        public static Point operator -(Point P)
        {
            return new Point(-P.x, -P.y);
        }

        public double AngleTo(Point direction)
        {
            if (this.Equals(new Point(0, 0)) || direction.Equals(new Point(0, 0)))
                return 0;
            return Math.Atan2(CrossProduct(direction), DotProduct(direction));
        }
    }
}
