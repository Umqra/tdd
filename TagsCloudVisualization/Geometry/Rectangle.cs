using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Geometry
{
    public struct Rectangle
    {
        public readonly Point BottomLeft;
        public readonly Point TopRight;

        public double Bottom => BottomLeft.Y;
        public double Left => BottomLeft.X;
        public double Top => TopRight.Y;
        public double Right => TopRight.X;

        public Point Center => (TopLeft + BottomRight) / 2;
        public Point TopLeft => new Point(Left, Top);
        public Point BottomRight => new Point(Right, Bottom);
        public Size Size => new Size(Right - Left, Top - Bottom);

        public double Area => (Top - Bottom) * (Right - Left);
        public bool IsEmpty => Area.ApproxEqualTo(0);

        public Rectangle(Point bottomLeft, Size size)
        {
            BottomLeft = bottomLeft;
            TopRight = bottomLeft + new Point(size.Width, size.Height);
        }

        public Rectangle(Point corner, Point oppositeCorner)
        {
            BottomLeft = new Point(Math.Min(corner.X, oppositeCorner.X), Math.Min(corner.Y, oppositeCorner.Y));
            TopRight = new Point(Math.Max(corner.X, oppositeCorner.X), Math.Max(corner.Y, oppositeCorner.Y));
        }

        public Rectangle? IntersectWith(Rectangle otherRectangle)
        {
            double newLeft = Math.Max(Left, otherRectangle.Left);
            double newRight = Math.Min(Right, otherRectangle.Right);
            double newBottom = Math.Max(Bottom, otherRectangle.Bottom);
            double newTop = Math.Min(Top, otherRectangle.Top);
            if (newLeft.ApproxGreater(newRight) || newBottom.ApproxGreater(newTop))
                return null;
            return new Rectangle(new Point(newLeft, newBottom), new Point(newRight, newTop));
        }

        private IEnumerable<Segment> MayBeSegment(Point a, Point b)
        {
            if (!a.Equals(b))
                yield return new Segment(a, b);
        }

        public IEnumerable<Segment> Sides =>
            MayBeSegment(BottomRight, TopRight)
                .Concat(MayBeSegment(TopRight, TopLeft))
                .Concat(MayBeSegment(TopLeft, BottomLeft))
                .Concat(MayBeSegment(BottomLeft, BottomRight));

        public IEnumerable<Point> Corners
        {
            get
            {
                yield return BottomLeft;
                yield return BottomRight;
                yield return TopRight;
                yield return TopLeft;
            }
        }

        public static explicit operator RectangleF(Rectangle rectangle)
        {
            return new RectangleF((PointF)rectangle.BottomLeft, (SizeF)rectangle.Size);
        }

        public static explicit operator System.Drawing.Rectangle(Rectangle rectangle)
        {
            return new System.Drawing.Rectangle((System.Drawing.Point)rectangle.BottomLeft,
                (System.Drawing.Size)rectangle.Size);
        }

        public static explicit operator Rectangle(System.Drawing.Rectangle rectangle)
        {
            return new Rectangle(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
        }

        public static explicit operator Rectangle(RectangleF rectangle)
        {
            return new Rectangle(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
        }

        public bool Equals(Rectangle other)
        {
            return BottomLeft.Equals(other.BottomLeft) && TopRight.Equals(other.TopRight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rectangle && Equals((Rectangle)obj);
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
                return (BottomLeft.GetHashCode() * 397) ^ TopRight.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"Rectangle({BottomLeft}, {TopRight})";
        }

        public bool Contains(Point p)
        {
            return Left.ApproxLessOrEqualTo(p.X) && p.X.ApproxLessOrEqualTo(Right) &&
                   Bottom.ApproxLessOrEqualTo(p.Y) && p.Y.ApproxLessOrEqualTo(Top);
        }

        public bool Touches(Rectangle rectangle)
        {
            var intersectionOrNull = IntersectWith(rectangle);
            if (intersectionOrNull == null)
                return false;
            var intersection = intersectionOrNull.Value;
            return intersection.Left.ApproxEqualTo(intersection.Right) ||
                   intersection.Bottom.ApproxEqualTo(intersection.Top);
        }

        public static Rectangle? BoundingBoxOf(IEnumerable<Point> allCorners)
        {
            var enumerated = allCorners.ToList();
            if (enumerated.Count == 0)
                return null;
            return new Rectangle(
                new Point(enumerated.Select(p => p.X).Min(), enumerated.Select(p => p.Y).Min()),
                new Point(enumerated.Select(p => p.X).Max(), enumerated.Select(p => p.Y).Max()));
        }
    }
}