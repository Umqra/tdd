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
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }

        public double Top => TopLeft.y;
        public double Bottom => BottomRight.y;
        public double Left => TopLeft.x;
        public double Right => BottomRight.x;

        public Point Center => (TopLeft + BottomRight) / 2;
        public Point TopRight => new Point(Right, Top);
        public Point BottomLeft => new Point(Bottom, Left);
        public Size Size => new Size(Right - Left, Top - Bottom);

        public double Area => (Top - Bottom) * (Right - Left);
        public bool IsEmpty => Area.EqualTo(0);

        public Rectangle(Point center, Size size)
        {
            TopLeft = center + new Point(-size.Width, size.Height);
            BottomRight = center + new Point(size.Width, -size.Height);
        }

        public Rectangle(Point center, double width, double height)
            : this(center, new Size(width, height))
        {
        }

        public Rectangle(Point corner, Point oppositeCorner)
        {
            TopLeft = new Point(Math.Min(corner.x, oppositeCorner.x), Math.Max(corner.y, oppositeCorner.y));
            BottomRight = new Point(Math.Max(corner.x, oppositeCorner.x), Math.Min(corner.y, oppositeCorner.y)); ;
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

        public System.Drawing.Rectangle ToDrawingRectangle()
        {
            return new System.Drawing.Rectangle(TopLeft.ToDrawingPoint(), Size.ToDrawingSizeF().ToSize());
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
    }
}
