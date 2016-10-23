using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Geometry.Rectangle;
using Size = Geometry.Size;

namespace TagsCloudVisualization
{
    public interface ICloudFormatter
    {
        Size MeasureString(string tag, Graphics graphics);
        void PutTag(string tag, Rectangle tagPlace, Graphics graphics);
    }
}
