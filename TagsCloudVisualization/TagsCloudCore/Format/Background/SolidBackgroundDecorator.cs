using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudCore.Format.Background
{
    public class SolidBackgroundDecorator : IBackgroundDecorator
    {
        private Color BackgroundColor { get; }
        public SolidBackgroundDecorator(Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
        }
        public void DecorateBackground(Graphics graphics)
        {
            graphics.Clear(BackgroundColor);
        }
    }
}
