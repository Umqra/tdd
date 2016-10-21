using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Line
    {
        public Point A { get; set; }
        public Point B { get; set; }

        public Point Direction => B - A;

        public Line(Point A, Point B)
        {
            if (A.Equals(B))
                throw new ArgumentException("Pivot points can't coincide");
            this.A = A;
            this.B = B;
        }

        public bool ParallelTo(Line otherLine)
        {
            return Direction.CollinearTo(otherLine.Direction);
        }

        public bool ContainsPoint(Point P)
        {
            return (P - A).CollinearTo(Direction);
        }

        public Point IntersectsWith(Line otherLine)
        {
            if (this.ParallelTo(otherLine))
                return null;
            double k = (A - otherLine.A).CrossProduct(Direction) / otherLine.Direction.CrossProduct(Direction);
            return otherLine.A + k * otherLine.Direction;
        }

        protected bool Equals(Line other)
        {
            return Direction.CollinearTo(other.Direction) && other.ContainsPoint(A);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Line)obj);
        }

        public override string ToString()
        {
            return $"Line({A}, {B})";
        }
    }
}
