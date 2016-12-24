using System.Drawing;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format.Tag.Decorating
{
    public class FadedColorTagsDecorator : ITagsDecorator
    {
        private IFadedColorTagsDecoratorSettings Settings { get; }
        public FadedColorTagsDecorator(IFadedColorTagsDecoratorSettings settings)
        {
            Settings = settings;
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
            var bright = Settings.BrightColor;
            var fade = Settings.FadeColor;
            return Color.FromArgb(
                (int)(bright.A * (1 - k) + fade.A * k),
                (int)(bright.R * (1 - k) + fade.R * k),
                (int)(bright.G * (1 - k) + fade.G * k),
                (int)(bright.B * (1 - k) + fade.B * k)
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
