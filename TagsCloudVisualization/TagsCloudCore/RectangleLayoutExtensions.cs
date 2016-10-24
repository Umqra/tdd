using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace TagsCloudCore
{
    public static class RectangleLayoutExtensions
    {
        public static Bitmap CreateImage(this List<Geometry.Rectangle> rectangles, int scaleFactor, Brush brush)
        {
            var boundingBox = Geometry.Rectangle.BoundingBoxOf(rectangles.SelectMany(rectangle => rectangle.Corners));
            var image = new Bitmap(
                (int)Math.Round(boundingBox.Size.Width) * scaleFactor,
                (int)Math.Round(boundingBox.Size.Height) * scaleFactor);

            var graphics = Graphics.FromImage(image);
            graphics.FillRectangles(brush,
                rectangles
                    .Select(rectangle =>
                        new Geometry.Rectangle(
                            (rectangle.BottomLeft - boundingBox.BottomLeft) * scaleFactor,
                            rectangle.Size * scaleFactor))
                    .Select(rectangle => (RectangleF)rectangle)
                    .ToArray());
            return image;
        }
    }
}
