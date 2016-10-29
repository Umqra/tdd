using System;

namespace Geometry
{
    public class Segment
    {
        public Point A { get; }
        public Point B { get; }

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

        private Point PointIfContainsElseNull(Point p)
        {
            if (p != null && Contains(p))
                return p;
            return null;
        }

        public Point IntersectWith(Line line)
        {
            Point intersection = line.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public Point IntersectWith(Ray ray)
        {
            Point intersection = ray.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public Point IntersectWith(Segment other)
        {
            Point intersection = other.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public double DistanceTo(Point p)
        {
            if ((p - A).DotProduct(B - A).ApproxGreaterOrEqualTo(0) &&
                (p - B).DotProduct(A - B).ApproxGreaterOrEqualTo(0))
                return BaseLine.DistanceTo(p);
            return Math.Min(p.DistanceTo(A), p.DistanceTo(B));
        }

        protected bool Equals(Segment other)
        {
            return (Equals(A, other.A) && Equals(B, other.B)) ||
                   (Equals(A, other.B) && Equals(B, other.A));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Segment)obj);
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
                return ((A?.GetHashCode() ?? 0) * 397) ^ (B?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return $"Segment({A}, {B})";
        }
    }
}
