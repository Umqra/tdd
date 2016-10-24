using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
#pragma warning disable 659

namespace Geometry
{
    public class Size
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public static Point operator +(Point P, Size size)
        {
            return new Point(P.x + size.Width, P.y + size.Height);
        }

        public static Point operator -(Point P, Size size)
        {
            return new Point(P.x - size.Width, P.y - size.Height);
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

        public static explicit operator System.Drawing.SizeF(Size size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }

        public static explicit operator System.Drawing.Size(Size size)
        {
            return ((SizeF)size).ToSize();
        }

        public static explicit operator Size(System.Drawing.SizeF size)
        {
            return new Size(size.Width, size.Height);
        }

        public static explicit operator Size(System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        protected bool Equals(Size other)
        {
            return Width.EqualTo(other.Width) && Height.EqualTo(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Size)obj);
        }
    }
}
