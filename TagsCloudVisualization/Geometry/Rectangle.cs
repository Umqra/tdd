using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Rectangle
    {
        public Point BottomLeft { get; set; }
        public Point TopRight { get; set; }

        public double Bottom => BottomLeft.y;
        public double Left => BottomLeft.x;
        public double Top => TopRight.y;
        public double Right => TopRight.x;

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
            BottomLeft = new Point(Math.Min(corner.x, oppositeCorner.x), Math.Min(corner.y, oppositeCorner.y));
            TopRight = new Point(Math.Max(corner.x, oppositeCorner.x), Math.Max(corner.y, oppositeCorner.y)); ;
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

        public IEnumerable<Segment> Sides
        {
            get
            {
                yield return new Segment(BottomRight, TopRight);
                yield return new Segment(TopRight, TopLeft);
                yield return new Segment(TopLeft, BottomLeft);
                yield return new Segment(BottomLeft, BottomRight);
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rectangle)obj);
        }

        public override string ToString()
        {
            return $"RT[{BottomLeft},{TopRight}]";
        }

        public bool Contains(Point center)
        {
            return Left.LessThanOrEqualTo(center.x) && center.x.LessThanOrEqualTo(Right) &&
                   Bottom.LessThanOrEqualTo(center.y) && center.y.LessThanOrEqualTo(Top);
        }

        public bool Touches(Rectangle rectangle)
        {
            var intersection = IntersectWith(rectangle);
            return intersection != null &&
                   (intersection.Left.EqualTo(intersection.Right) || intersection.Bottom.EqualTo(intersection.Top));
        }
    }
}
