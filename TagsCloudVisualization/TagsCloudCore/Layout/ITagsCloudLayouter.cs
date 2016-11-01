using System.Collections.Generic;
using Geometry;

namespace TagsCloudCore.Layout
{
    public interface ITagsCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        IEnumerable<Rectangle> GetRectangles();
    }
}
