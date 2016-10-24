using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
#pragma warning disable 659

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

        public bool ParallelTo(Line other)
        {
            return Direction.CollinearTo(other.Direction);
        }

        public bool Contains(Point P)
        {
            return (P - A).CollinearTo(Direction);
        }

        public Point IntersectWith(Line other)
        {
            if (this.ParallelTo(other))
                return null;
            double k = (A - other.A).CrossProduct(Direction) / other.Direction.CrossProduct(Direction);
            return other.A + k * other.Direction;
        }

        public double DistanceTo(Point P)
        {
            return Math.Abs((B - A).CrossProduct(P - A)) / (B - A).Length;
        }

        public Point PerpendicularFrom(Point P)
        {
            return A + Direction * (P - A).DotProduct(Direction) / (Direction.DotProduct(Direction));
        }

        protected bool Equals(Line other)
        {
            return Direction.CollinearTo(other.Direction) && other.Contains(A);
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
