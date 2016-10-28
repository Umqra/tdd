using System.Drawing;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format
{
    public interface ITagsDecorator
    {
        void DecorateTag(string tag, Font tagFont, Rectangle tagPlace, Graphics graphics);
    }
}