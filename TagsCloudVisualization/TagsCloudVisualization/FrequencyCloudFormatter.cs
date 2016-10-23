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
    public class FrequencyCloudFormatter : ICloudFormatter
    {
        public FontFamily FontFamily { get; set; }
        public float MaxEmSize { get; set; }
        public Brush Brush { get; set; }
        public Dictionary<string, int> TagsFrequency;
        private int maxFrequency = 0;
        public FrequencyCloudFormatter(FontFamily fontFamily, float maxEmSize, Brush brush, IEnumerable<string> tags)
        {
            FontFamily = fontFamily;
            MaxEmSize = maxEmSize;
            Brush = brush;

            TagsFrequency = new Dictionary<string, int>();
            foreach (var tag in tags)
            {
                if (!TagsFrequency.ContainsKey(tag))
                    TagsFrequency[tag] = 0;
                TagsFrequency[tag]++;
                maxFrequency = Math.Max(maxFrequency, TagsFrequency[tag]);
            }
        }

        private int GetFrequence(string tag)
        {
            return TagsFrequency.ContainsKey(tag) ? TagsFrequency[tag] : 0;
        }

        private Font GetTagFont(string tag)
        {
            var emSize = MaxEmSize * Math.Pow(((double)GetFrequence(tag) + 1) / (maxFrequency + 1), 0.7);
            return new Font(FontFamily, (float)emSize);
        }

        public Size MeasureString(string tag, Graphics graphics)
        {
            var currentFont = GetTagFont(tag);
            return (Size)graphics.MeasureString(tag, currentFont);
        }

        public void PutTag(string tag, Rectangle tagPlace, Graphics graphics)
        {
            var currentFont = GetTagFont(tag);
            graphics.DrawString(tag, currentFont, Brush, (System.Drawing.RectangleF)tagPlace);
        }
    }
}
