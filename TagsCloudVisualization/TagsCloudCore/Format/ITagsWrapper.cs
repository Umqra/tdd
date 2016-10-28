using System.Drawing;
using Size = Geometry.Size;

namespace TagsCloudCore.Format
{
    // CR: 'Cloud' again
    // CR: I think this interface would benifit from some improvements
    // It tries to contain two separate, indepented responsibilities
    // Look at the logical flow
    // tags -> (construct) -> formatter -> (generate size) -> layouter -> (generate rectangle) -> formatter
    // It seems like formatter on the left can be separated from the formatter on the right
    public interface ITagsWrapper
    {
        Font GetTagFont(string tag);
        Size MeasureTag(string tag, Graphics graphics);
    }
}