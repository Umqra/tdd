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
            throw new NotImplementedException();
        }

        public bool IntersectsWith(Line line)
        {
            throw new NotImplementedException();
        }

        public bool IntersectsWith(Ray ray)
        {
            throw new NotImplementedException();
        }

        public bool IntersectsWith(Segment otherSegment)
        {
            throw new NotImplementedException();
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
