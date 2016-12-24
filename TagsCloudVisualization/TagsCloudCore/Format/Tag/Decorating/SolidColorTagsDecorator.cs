using System.Drawing;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format.Tag.Decorating
{
    public class SolidColorTagsDecorator : ITagsDecorator
    {
        private ISolidColorTagsDecoratorSettings Settings { get; }
        public SolidColorTagsDecorator(ISolidColorTagsDecoratorSettings settings)
        {
            Settings = settings;
        }
        public void DecorateTag(string tag, Font tagFont, Rectangle tagPlace, Graphics graphics)
        {
            graphics.DrawString(tag, tagFont, new SolidBrush(Settings.Color), (RectangleF)tagPlace);
        }
    }
}
