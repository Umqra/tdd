using System;

//TODO: warning 659

namespace Geometry
{
    public class Line
    {
        public Point A { get; set; }
        public Point B { get; set; }

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

        public Point IntersectWith(Line other)
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
            return A + Direction * (p - A).DotProduct(Direction) / (Direction.DotProduct(Direction));
        }

        protected bool Equals(Line other)
        {
            return Direction.CollinearTo(other.Direction) && other.Contains(A);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Line)obj);
        }

        public override string ToString()
        {
            return $"Line({A}, {B})";
        }
    }
}
