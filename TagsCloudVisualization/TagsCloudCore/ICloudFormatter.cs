using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Geometry.Rectangle;
using Size = Geometry.Size;

namespace TagsCloudCore
{
    // CR: 'Cloud' again
    // CR: I think this interface would benifit from some improvements
    // It tries to contain two separate, indepented responsibilities
    // Look at the logical flow
    // tags -> (construct) -> formatter -> (generate size) -> layouter -> (generate rectangle) -> formatter
    // It seems like formatter on the left can be separated from the formatter on the right
    public interface ICloudFormatter
    {
        Size MeasureString(string tag, Graphics graphics);
        void PutTag(string tag, Rectangle tagPlace, Graphics graphics);
    }
}
