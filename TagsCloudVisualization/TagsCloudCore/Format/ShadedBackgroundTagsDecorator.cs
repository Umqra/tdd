using System.Drawing;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format
{
    public class ShadedBackgroundTagsDecorator : ITagsDecorator
    {
        public Color ShadeColor { get; set; }

        public ShadedBackgroundTagsDecorator(Color shadeColor)
        {
            ShadeColor = shadeColor;
        }

        private Geometry.Point GetImageCenter(Graphics graphics)
        {
            return new Geometry.Point(0, 0) + (Geometry.Size)graphics.VisibleClipBounds.Size / 2;
        }

        private double GetMaxDistanceFromCenter(Graphics graphics)
        {
            return GetImageCenter(graphics).Length;
        }

        public void DecorateTag(string tag, Font tagFont, Rectangle tagPlace, Graphics graphics)
        {
            double distanceToTag = GetImageCenter(graphics).DistanceTo(tagPlace.Center);

            int alphaChannel = (int)(distanceToTag / GetMaxDistanceFromCenter(graphics) * 255);
            var brush = new SolidBrush(Color.FromArgb(alphaChannel, ShadeColor.R, ShadeColor.G, ShadeColor.B));
            graphics.FillRectangle(brush, (RectangleF)tagPlace);
        }
    }
}
