using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format
{
    public class SolidColorTagsDecorator : ITagsDecorator
    {
        public Color Color { get; set; }
        public SolidColorTagsDecorator(Color color)
        {
            Color = color;
        }
        public void DecorateTag(string tag, Font tagFont, Rectangle tagPlace, Graphics graphics)
        {
            graphics.DrawString(tag, tagFont, new SolidBrush(Color), (RectangleF)tagPlace);
        }
    }
}
