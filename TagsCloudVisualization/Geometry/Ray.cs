using System;

namespace Geometry
{
    public struct Ray
    {
        public readonly Point From;
        public readonly Point To;

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

        public Point? IntersectWith(Ray other)
        {
            Point? intersection = other.IntersectWith(BaseLine);
            return PointIfContainsElseNull(intersection);
        }

        public double DistanceTo(Point p)
        {
            if ((p - From).DotProduct(Direction).ApproxGreaterOrEqualTo(0))
                return BaseLine.DistanceTo(p);
            return From.DistanceTo(p);
        }

        public bool Equals(Ray other)
        {
            return From.Equals(other.From) && Direction.HasSameDirectionAs(other.Direction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Ray && Equals((Ray)obj);
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
                return (From.GetHashCode() * 397) ^ To.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"Ray({From}, {To})";
        }
    }
}
