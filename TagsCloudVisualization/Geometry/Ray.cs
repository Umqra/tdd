using System;

//TODO: warning 659

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

        public bool Contains(Point p)
        {
            return Direction.HasSameDirectionAs(p - From);
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

        public Point IntersectWith(Ray other)
        {
            Point intersection = other.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public double DistanceTo(Point p)
        {
            if ((p - From).DotProduct(Direction).GreaterThanOrEqualTo(0))
                return BaseLine.DistanceTo(p);
            return From.DistanceTo(p);
        }

        protected bool Equals(Ray other)
        {
            return From.Equals(other.From) && Direction.HasSameDirectionAs(other.Direction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Ray)obj);
        }
    }
}
