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

        public System.Drawing.PointF ToDrawingPointF()
        {
            return new System.Drawing.PointF((float)x, (float)y);
        }

        public System.Drawing.Point ToDrawingPoint()
        {
            return new System.Drawing.Point((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}
