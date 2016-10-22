using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Segment
    {
        public Point A { get; set; }
        public Point B { get; set; }

        public Line BaseLine => new Line(A, B);

        public Segment(Point A, Point B)
        {
            if (A.Equals(B))
                throw new ArgumentException("Segments endpoints can't coincide");
            this.A = A;
            this.B = B;
        }

        public bool ContainsPoint(Point P)
        {
            return BaseLine.ContainsPoint(P) && (B - A).HasSameDirectionAs(P - A) && (A - B).HasSameDirectionAs(P - B);
        }

        public Point IntersectWith(Line line)
        {
            Point P = line.IntersectWith(BaseLine);
            if (P != null && ContainsPoint(P))
                return P;
            return null;
        }

        public Point IntersectWith(Ray ray)
        {
            Point P = ray.IntersectWith(BaseLine);
            if (P != null && ContainsPoint(P))
                return P;
            return null;
        }

        public Point IntersectWith(Segment otherSegment)
        {
            Point P = otherSegment.IntersectWith(BaseLine);
            if (P != null && ContainsPoint(P))
                return P;
            return null;
        }

        public double DistanceTo(Point P)
        {
            if ((P - A).DotProduct(B - A).GreaterThanOrEqualTo(0) &&
                (P - B).DotProduct(A - B).GreaterThanOrEqualTo(0))
                return BaseLine.DistanceTo(P);
            return Math.Min(P.DistanceTo(A), P.DistanceTo(B));
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Segment)obj);
        }
        
    }
}
