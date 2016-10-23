using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
