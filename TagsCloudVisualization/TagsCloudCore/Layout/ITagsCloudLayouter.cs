using System.Collections.Generic;
using Geometry;

namespace TagsCloudCore.Layout
{
    // CR: Cloud is too ambiguous. Be consistent, use TagsCloud everywhere
    public interface ITagsCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        // CR: Why do you need List? What if you want to use some kind of tree internally to place tags faster?
        List<Rectangle> Rectangles { get; }
    }
}
