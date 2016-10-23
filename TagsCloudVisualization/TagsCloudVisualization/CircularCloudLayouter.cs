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
            Center = (Point)center;
        }

        public System.Drawing.Rectangle PutNextRectangle(System.Drawing.Size drawingSize)
        {
            var rectangleSize = (Size)drawingSize;
            if (LastRectangle != null)
                LastRectangle = new Rectangle(LastRectangle.TopRight, rectangleSize);
            else
                LastRectangle = new Rectangle(Center - rectangleSize / 2, rectangleSize);
            return (System.Drawing.Rectangle)LastRectangle;
        }
    }
}
