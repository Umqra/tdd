using System.Drawing;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format.Tag.Decorating
{
    public interface IShadedBackgroundTagsDecoratorSettings
    {
        Color ShadeColor { get; }
    }
    public class ShadedBackgroundTagsDecorator : ITagsDecorator
    {
        private IShadedBackgroundTagsDecoratorSettings Settings { get; }
        public ShadedBackgroundTagsDecorator(IShadedBackgroundTagsDecoratorSettings settings)
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

        public void DecorateTag(string tag, Font tagFont, Rectangle tagPlace, Graphics graphics)
        {
            double distanceToTag = GetImageCenter(graphics).DistanceTo(tagPlace.Center);

            int alphaChannel = (int)(distanceToTag / GetMaxDistanceFromCenter(graphics) * 255);
            var color = Settings.ShadeColor;
            var brush = new SolidBrush(Color.FromArgb(alphaChannel, color.R, color.G, color.B));
            graphics.FillRectangle(brush, (RectangleF)tagPlace);
        }
    }
}
