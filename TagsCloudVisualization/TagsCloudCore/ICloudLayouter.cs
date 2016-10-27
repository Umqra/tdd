using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace TagsCloudCore
{
    // CR: Cloud is too ambiguous. Be consistent, use TagsCloud everywhere
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        // CR: Why do you need List? What if you want to use some kind of tree internally to place tags faster?
        List<Rectangle> Rectangles { get; }
    }
}
