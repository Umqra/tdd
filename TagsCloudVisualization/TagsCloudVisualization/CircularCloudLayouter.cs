using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; set; }
        public Rectangle? LastRectangle { get; set; }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (LastRectangle.HasValue)
                LastRectangle = new Rectangle(LastRectangle.Value.Location + LastRectangle.Value.Size, rectangleSize);
            else
                LastRectangle = new Rectangle(Center, rectangleSize);
            return LastRectangle.Value;
        }
    }
}
