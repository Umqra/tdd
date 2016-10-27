using System;
using System.Collections.Generic;
using System.Linq;

//TODO: warning 659

namespace Geometry
{
    public class Rectangle
    {
        public Point BottomLeft { get; set; }
        public Point TopRight { get; set; }

        public double Bottom => BottomLeft.Y;
        public double Left => BottomLeft.X;
        public double Top => TopRight.Y;
        public double Right => TopRight.X;

        public Point Center => (TopLeft + BottomRight) / 2;
        public Point TopLeft => new Point(Left, Top);
        public Point BottomRight => new Point(Right, Bottom);
        public Size Size => new Size(Right - Left, Top - Bottom);

        public double Area => (Top - Bottom) * (Right - Left);
        public bool IsEmpty => Area.EqualTo(0);

        public Rectangle(Point bottomLeft, Size size)
        {
            BottomLeft = bottomLeft;
            TopRight = bottomLeft + new Point(size.Width, size.Height);
        }

        public Rectangle(Point bottomLeft, double width, double height)
            : this(bottomLeft, new Size(width, height))
        {
        }

        public Rectangle(Point corner, Point oppositeCorner)
        {
            BottomLeft = new Point(Math.Min(corner.X, oppositeCorner.X), Math.Min(corner.Y, oppositeCorner.Y));
            TopRight = new Point(Math.Max(corner.X, oppositeCorner.X), Math.Max(corner.Y, oppositeCorner.Y));
        }

        public Rectangle IntersectWith(Rectangle otherRectangle)
        {
            double newLeft = Math.Max(Left, otherRectangle.Left);
            double newRight = Math.Min(Right, otherRectangle.Right);
            double newTop = Math.Min(Top, otherRectangle.Top);
            double newBottom = Math.Max(Bottom, otherRectangle.Bottom);
            if (newLeft.GreaterThan(newRight) || newBottom.GreaterThan(newTop))
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

        public static explicit operator System.Drawing.RectangleF(Rectangle rectangle)
        {
            return new System.Drawing.RectangleF((System.Drawing.PointF)rectangle.BottomLeft, (System.Drawing.SizeF)rectangle.Size);
        }

        public static explicit operator System.Drawing.Rectangle(Rectangle rectangle)
        {
            return new System.Drawing.Rectangle((System.Drawing.Point)rectangle.BottomLeft, (System.Drawing.Size)rectangle.Size);
        }

        public static explicit operator Rectangle(System.Drawing.Rectangle rectangle)
        {
            return new Rectangle(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
        }

        public static explicit operator Rectangle(System.Drawing.RectangleF rectangle)
        {
            return new Rectangle(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
        }

        protected bool Equals(Rectangle other)
        {
            return Equals(TopLeft, other.TopLeft) && Equals(BottomRight, other.BottomRight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Rectangle)obj);
        }

        public override string ToString()
        {
            return $"RT[{BottomLeft},{TopRight}]";
        }

        public bool Contains(Point p)
        {
            return Left.LessThanOrEqualTo(p.X) && p.X.LessThanOrEqualTo(Right) &&
                   Bottom.LessThanOrEqualTo(p.Y) && p.Y.LessThanOrEqualTo(Top);
        }

        public bool Touches(Rectangle rectangle)
        {
            var intersection = IntersectWith(rectangle);
            return intersection != null &&
                   (intersection.Left.EqualTo(intersection.Right) || intersection.Bottom.EqualTo(intersection.Top));
        }

        public static Rectangle BoundingBoxOf(IEnumerable<Point> allCorners)
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
