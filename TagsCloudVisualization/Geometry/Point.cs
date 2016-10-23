using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
#pragma warning disable CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
    public class Point
#pragma warning restore CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
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

        public double DistanceTo(Point point)
        {
            return (this - point).Length;
        }

        protected bool Equals(Point other)
        {
            return x.EqualTo(other.x) && y.EqualTo(other.y);
        }

#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
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
        
        public static explicit operator System.Drawing.PointF(Point point)
        {
            return new System.Drawing.PointF((float)point.x, (float)point.y);
        }

        public static explicit operator System.Drawing.Point(Point point)
        {
            return new System.Drawing.Point((int)Math.Round(point.x), (int)Math.Round(point.y));
        }

        public static explicit operator Point(System.Drawing.PointF point)
        {
            return new Point(point.X, point.Y);
        }

        public static explicit operator Point(System.Drawing.Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static Point operator -(Point point)
        {
            return new Point(-point.x, -point.y);
        }

        public double AngleTo(Point direction)
        {
            if (this.Equals(new Point(0, 0)) || direction.Equals(new Point(0, 0)))
                return 0;
            return Math.Atan2(CrossProduct(direction), DotProduct(direction));
        }
    }
}
