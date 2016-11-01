using System;
using System.Collections.Generic;
using System.Drawing;
using Size = Geometry.Size;

namespace TagsCloudCore.Format
{
    public class FrequencyTagsCloudWrapper : ITagsWrapper
    {
        public const double FontSizeTuner = 0.7;

        public FontFamily FontFamily { get; set; }
        public float MaxFontSizeInEm { get; set; }
        public Dictionary<string, int> TagsFrequency;
        private int MaxFrequency { get; }

        public FrequencyTagsCloudWrapper(FontFamily fontFamily, float maxFontSizeInEm, IEnumerable<string> tags)
        {
            FontFamily = fontFamily;
            MaxFontSizeInEm = maxFontSizeInEm;
        
            TagsFrequency = new Dictionary<string, int>();
            foreach (var tag in tags)
            {
                if (!TagsFrequency.ContainsKey(tag))
                    TagsFrequency[tag] = 0;
                TagsFrequency[tag]++;
                MaxFrequency = Math.Max(MaxFrequency, TagsFrequency[tag]);
            }
        }

        private int GetFrequency(string tag)
        {
            return TagsFrequency.ContainsKey(tag) ? TagsFrequency[tag] : 0;
        }

        public Font GetTagFont(string tag)
        {
            double frequencyRatio = ((double)GetFrequency(tag) + 1) / (MaxFrequency + 1);
            // +1 for unknown tags, now it's size negligible, but not zero

            var fontSize = MaxFontSizeInEm * Math.Pow(frequencyRatio, FontSizeTuner);
            return new Font(FontFamily, (float)fontSize);
        }

        public Size MeasureTag(string tag, Graphics graphics)
        {
            var currentFont = GetTagFont(tag);
            return (Size)graphics.MeasureString(tag, currentFont);
        }
    }
}
