using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
#pragma warning disable 659

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

        public bool Contains(Point P)
        {
            return BaseLine.Contains(P) &&
                   (B - A).HasSameDirectionAs(P - A) &&
                   (A - B).HasSameDirectionAs(P - B);
        }

        private Point PointIfContainsElseNull(Point P)
        {
            if (P != null && Contains(P))
                return P;
            return null;
        }

        public Point IntersectWith(Line line)
        {
            Point P = line.IntersectWith(BaseLine);
            return PointIfContainsElseNull(P);
        }

        public Point IntersectWith(Ray ray)
        {
            Point P = ray.IntersectWith(BaseLine);
            return PointIfContainsElseNull(P);
        }

        public Point IntersectWith(Segment other)
        {
            Point P = other.IntersectWith(BaseLine);
            return PointIfContainsElseNull(P);
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
