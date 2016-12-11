using System.Drawing;
using Size = Geometry.Size;

namespace TagsCloudCore.Format.Tag.Wrapping
{
    public interface ITagsWrapper
    {
        Font GetTagFont(string tag);
        Size MeasureTag(string tag, Graphics graphics);
    }
}