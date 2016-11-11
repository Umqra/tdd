using System;

namespace Geometry
{
    public struct Line
    {
        public readonly Point A;
        public readonly Point B;

        public Point Direction => B - A;

        public Line(Point a, Point b)
        {
            if (a.Equals(b))
                throw new ArgumentException("Pivot points can't coincide");
            A = a;
            B = b;
        }

        public bool ParallelTo(Line other)
        {
            return Direction.CollinearTo(other.Direction);
        }

        public bool Contains(Point p)
        {
            return (p - A).CollinearTo(Direction);
        }

        public Point? IntersectWith(Line other)
        {
            if (ParallelTo(other))
                return null;
            double k = (A - other.A).CrossProduct(Direction) / other.Direction.CrossProduct(Direction);
            return other.A + k * other.Direction;
        }

        public double DistanceTo(Point p)
        {
            return Math.Abs((B - A).CrossProduct(p - A)) / (B - A).Length;
        }

        public Point PerpendicularFrom(Point p)
        {
            return A + Direction * (p - A).DotProduct(Direction) / Direction.DotProduct(Direction);
        }

        public bool Equals(Line other)
        {
            return Direction.CollinearTo(other.Direction) && other.Contains(A);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Line && Equals((Line)obj);
        }

        /// <summary>
        /// Current GetHashCode implementation not consistent with Equals method (because of 
        /// <see cref="Point"/>.<see cref="Point.GetHashCode()"/> 
        /// implementation).
        /// <para>Use it only where it really needed and with caution.</para>
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return (A.GetHashCode() * 397) ^ B.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"Line({A}, {B})";
        }
    }
}