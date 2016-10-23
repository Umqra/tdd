using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
#pragma warning disable 659

namespace Geometry
{
    public class Ray
    {
        public Point From { get; set; }
        public Point To { get; set; }

        public Point Direction => To - From;
        public Line BaseLine => new Line(From, To);

        public Ray(Point from, Point to)
        {
            if (from.Equals(to))
                throw new ArgumentException("Pivot points can't coincide");
            From = from;
            To = to;
        }

        public bool Contains(Point P)
        {
            return Direction.HasSameDirectionAs(P - From);
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

        public Point IntersectWith(Ray other)
        {
            Point P = other.IntersectWith(BaseLine);
            return PointIfContainsElseNull(P);
        }

        public double DistanceTo(Point P)
        {
            if ((P - From).DotProduct(Direction).GreaterThanOrEqualTo(0))
                return BaseLine.DistanceTo(P);
            return From.DistanceTo(P);
        }

        protected bool Equals(Ray other)
        {
            return From.Equals(other.From) && Direction.HasSameDirectionAs(other.Direction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Ray)obj);
        }
    }
}
