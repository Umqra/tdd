using System;

//TODO: warning 659

namespace Geometry
{
    public class Segment
    {
        public Point A { get; set; }
        public Point B { get; set; }

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
            if ((p - A).DotProduct(B - A).GreaterThanOrEqualTo(0) &&
                (p - B).DotProduct(A - B).GreaterThanOrEqualTo(0))
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
        
    }
}
