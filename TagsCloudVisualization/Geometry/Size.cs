using System.Drawing;

namespace Geometry
{
    public struct Size
    {
        public readonly double Width;

        public readonly double Height;

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public static Point operator +(Point p, Size size)
        {
            return new Point(p.X + size.Width, p.Y + size.Height);
        }

        public static Point operator -(Point p, Size size)
        {
            return new Point(p.X - size.Width, p.Y - size.Height);
        }

        public static Size operator +(Size first, Size second)
        {
            return new Size(first.Width + second.Width, first.Height + second.Height);
        }

        public static Size operator /(Size size, double k)
        {
            return new Size(size.Width / k, size.Height / k);
        }

        public static Size operator *(Size size, double k)
        {
            return new Size(size.Width * k, size.Height * k);
        }

        public static Size operator *(double k, Size size)
        {
            return size * k;
        }

        public static explicit operator SizeF(Size size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }

        public static explicit operator System.Drawing.Size(Size size)
        {
            return ((SizeF)size).ToSize();
        }

        public static explicit operator Size(SizeF size)
        {
            return new Size(size.Width, size.Height);
        }

        public static explicit operator Size(System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        public bool Equals(Size other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Size && Equals((Size)obj);
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
                return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
            }
        }
    }
}
