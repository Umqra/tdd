using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Size(System.Drawing.SizeF size) : this(size.Width, size.Height)
        {
        }

        public Size(System.Drawing.Size size) : this(size.Width, size.Height)
        {
        }

        public static explicit operator System.Drawing.SizeF(Size size)
        {
            return size.ToDrawingSizeF();
        }

        public static explicit operator System.Drawing.Size(Size size)
        {
            return size.ToDrawingSizeF().ToSize();
        }

        public static explicit operator Size(System.Drawing.SizeF size)
        {
            return new Size(size);
        }

        public static explicit operator Size(System.Drawing.Size size)
        {
            return new Size(size);
        }


        public static Point operator +(Point P, Size size)
        {
            return new Point(P.x + size.Width, P.y + size.Height);
        }

        public static Point operator +(Size size, Point P)
        {
            return P + size;
        }

        public System.Drawing.SizeF ToDrawingSizeF()
        {
            return new System.Drawing.SizeF((float)Width, (float)Height);
        }
    }
}
