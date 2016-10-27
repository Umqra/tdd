using System;

// CR: Do not use those
//! ReSharper disable InconsistentNaming
// TODO: warning disable 659
// fix problems they indicate

// CR: Make naming consistent again!
namespace Geometry
{
    // CR: Warning actually indicates serious problem
    public class Point
    {
        public readonly double X;
        public readonly double Y;

        public Point Orthogonal => new Point(-Y, X);
        public double Length => Math.Sqrt(X * X + Y * Y);

        public bool IsZero => X.EqualTo(0) && Y.EqualTo(0);

        public int Quater
        {
            get
            {
                // CR: Why create new object?
                if (IsZero)
                    return 0;
                if (X > 0 && Y >= 0)
                    return 1;
                if (X <= 0 && Y > 0)
                    return 2;
                if (X < 0 && Y <= 0)
                    return 3;
                if (X >= 0 && Y < 0)
                    return 4;
                throw new ArgumentException();
            }
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point p, double k)
        {
            return new Point(p.X * k, p.Y * k);
        }

        public static Point operator *(double k, Point p)
        {
            return new Point(p.X * k, p.Y * k);
        }

        public static Point operator /(Point p, double k)
        {
            return new Point(p.X / k, p.Y / k);
        }

        public double DotProduct(Point other)
        {
            return X * other.X + Y * other.Y;
        }

        public double CrossProduct(Point other)
        {
            return X * other.Y - Y * other.X;
        }

        public bool CollinearTo(Point other)
        {
            return CrossProduct(other).EqualTo(0);
        }

        public bool HasSameDirectionAs(Point other)
        {
            return CollinearTo(other) && DotProduct(other).GreaterThanOrEqualTo(0);
        }

        public Point Rotate(double angleInRadians)
        {
            double cosAngle = Math.Cos(angleInRadians);
            double sinAngle = Math.Sin(angleInRadians);
            return new Point(X * cosAngle - Y * sinAngle, X * sinAngle + Y * cosAngle);
        }

        public double DistanceTo(Point other)
        {
            return (this - other).Length;
        }

        protected bool Equals(Point other)
        {
            return X.EqualTo(other.X) && Y.EqualTo(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Point)obj);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
        
        public static explicit operator System.Drawing.PointF(Point p)
        {
            return new System.Drawing.PointF((float)p.X, (float)p.Y);
        }

        public static explicit operator System.Drawing.Point(Point p)
        {
            return new System.Drawing.Point((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        public static explicit operator Point(System.Drawing.PointF p)
        {
            return new Point(p.X, p.Y);
        }

        public static explicit operator Point(System.Drawing.Point p)
        {
            return new Point(p.X, p.Y);
        }

        public static Point operator -(Point p)
        {
            return new Point(-p.X, -p.Y);
        }

        public double AngleTo(Point direction)
        {
            if (IsZero || direction.IsZero)
                return 0;
            return Math.Atan2(CrossProduct(direction), DotProduct(direction));
        }
    }
}
