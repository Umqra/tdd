using System;

namespace Geometry
{
    public struct Segment
    {
        public readonly Point A;
        public readonly Point B;

        public Line BaseLine => new Line(A, B);

        public Segment(Point a, Point b)
        {
            if (a.Equals(b))
                throw new ArgumentException("Segments endpoints can't coincide");
            A = a;
            B = b;
        }

        public bool Contains(Point p)
        {
            return BaseLine.Contains(p) &&
                   (B - A).HasSameDirectionAs(p - A) &&
                   (A - B).HasSameDirectionAs(p - B);
        }

        private Point? PointIfContainsElseNull(Point? p)
        {
            if (p != null && Contains(p.Value))
                return p;
            return null;
        }

        public Point? IntersectWith(Line line)
        {
            Point? intersection = line.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public Point? IntersectWith(Ray ray)
        {
            Point? intersection = ray.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public Point? IntersectWith(Segment other)
        {
            Point? intersection = other.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public double DistanceTo(Point p)
        {
            if ((p - A).DotProduct(B - A).ApproxGreaterOrEqualTo(0) &&
                (p - B).DotProduct(A - B).ApproxGreaterOrEqualTo(0))
                return BaseLine.DistanceTo(p);
            return Math.Min(p.DistanceTo(A), p.DistanceTo(B));
        }

        public bool Equals(Segment other)
        {
            return (A.Equals(other.A) && B.Equals(other.B)) ||
                   (A.Equals(other.B) && B.Equals(other.A));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Segment && Equals((Segment)obj);
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
            return $"Segment({A}, {B})";
        }
    }
}
