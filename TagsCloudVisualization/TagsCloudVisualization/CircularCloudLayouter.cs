using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; set; }
        public Rectangle LastRectangle { get; set; }
        public CircularCloudLayouter(System.Drawing.Point center)
        {
            Center = center;
        }

        public System.Drawing.Rectangle PutNextRectangle(System.Drawing.Size drawingSize)
        {
            var rectangleSize = new Size(drawingSize);
            if (LastRectangle != null)
                LastRectangle = new Rectangle(LastRectangle.TopRight + LastRectangle.Size, rectangleSize);
            else
                LastRectangle = new Rectangle(Center, rectangleSize);
            return LastRectangle.ToDrawingRectangle();
        }
    }
}
