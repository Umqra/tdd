using System.Drawing;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format.Tag.Decorating
{
    public class FadedColorTagsDecorator : ITagsDecorator
    {
        public Color BrightColor { get; set; }
        public Color FadeColor { get; set; }

        public FadedColorTagsDecorator(Color brightColor, Color fadeColor)
        {
            BrightColor = brightColor;
            FadeColor = fadeColor;
        }

        private Geometry.Point GetImageCenter(Graphics graphics)
        {
            return new Geometry.Point(0, 0) + (Geometry.Size)graphics.VisibleClipBounds.Size / 2;
        }

        private double GetMaxDistanceFromCenter(Graphics graphics)
        {
            return GetImageCenter(graphics).Length;
        }

        private Color GetMixedColor(double k)
        {
            return Color.FromArgb(
                (int)(BrightColor.A * (1 - k) + FadeColor.A * k),
                (int)(BrightColor.R * (1 - k) + FadeColor.R * k),
                (int)(BrightColor.G * (1 - k) + FadeColor.G * k),
                (int)(BrightColor.B * (1 - k) + FadeColor.B * k)
            );
        }

        public void DecorateTag(string tag, Font tagFont, Rectangle tagPlace, Graphics graphics)
        {
            double distanceToTag = GetImageCenter(graphics).DistanceTo(tagPlace.Center);

            double k = distanceToTag / GetMaxDistanceFromCenter(graphics);

            var brush = new SolidBrush(GetMixedColor(k));
            graphics.DrawString(tag, tagFont, brush, (RectangleF)tagPlace);
        }
    }
}
