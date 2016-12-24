using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudCore.Errors;

namespace TagsCloudCore
{
    public static class RectangleLayoutExtensions
    {
        public static Result<Bitmap> CreateImage(this IEnumerable<Geometry.Rectangle> rectangles, int scaleFactor, Brush brush)
        {
            var enumeratedRectangles = rectangles.ToList();
            var boundingBoxOrNull = Geometry.Rectangle.BoundingBoxOf(enumeratedRectangles.SelectMany(rectangle => rectangle.Corners));
            if (!boundingBoxOrNull.HasValue)
                return Result.Fail<Bitmap>(
                    new NoRectanglesError("Can't draw rectangles layout from empty sequence of rectangles"));

            var boundingBox = boundingBoxOrNull.Value;
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
            return image.AsResult();
        }
    }
}
