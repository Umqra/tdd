using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static PointF GetCenter(this Rectangle rectangle)
        {
            return new PointF(
                (float)(rectangle.Left + rectangle.Right) / 2, 
                (float)(rectangle.Bottom + rectangle.Top) / 2
                );
        }
    }

    public static class PointExtensions
    {
        public static int GetQuarter(this PointF point)
        {
            if (point == new PointF(0, 0))
                return 0;
            if (point.Y >= 0 && point.X > 0)
                return 1;
            if (point.Y > 0 && point.X <= 0)
                return 2;
            if (point.Y <= 0 && point.X < 0)
                return 3;
            if (point.Y > 0 && point.X >= 0)
                return 4;
            throw new ArgumentException();
        }
    }
}
