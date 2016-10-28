using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudCore
{
    public static class RectangleLayoutExtensions
    {
        public static Bitmap CreateImage(this IEnumerable<Geometry.Rectangle> rectangles, int scaleFactor, Brush brush)
        {
            var enumeratedRectangles = rectangles.ToList();
            var boundingBox = Geometry.Rectangle.BoundingBoxOf(enumeratedRectangles.SelectMany(rectangle => rectangle.Corners));
            var image = new Bitmap(
                (int)Math.Round(boundingBox.Size.Width) * scaleFactor,
                (int)Math.Round(boundingBox.Size.Height) * scaleFactor);

            var graphics = Graphics.FromImage(image);
            graphics.FillRectangles(brush,
                enumeratedRectangles
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
