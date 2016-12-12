using System;
using System.Collections.Generic;
using System.Drawing;
using Size = Geometry.Size;

namespace TagsCloudCore.Format.Tag.Wrapping
{
    public class FrequencyTagsCloudWrapper : ITagsWrapper
    {
        private const double FontSizeTuner = 0.7;
        private readonly Dictionary<string, int> tagsFrequency;

        private Func<float, Font> FontCreator { get; }
        private float MaxFontEmSize { get; }
        private int MaxFrequency { get; }

        public FrequencyTagsCloudWrapper(Func<float, Font> fontCreator, float maxFontEmSize, IEnumerable<string> tags)
        {
            FontCreator = fontCreator;
            MaxFontEmSize = maxFontEmSize;

            tagsFrequency = new Dictionary<string, int>();
            foreach (var tag in tags)
            {
                if (!tagsFrequency.ContainsKey(tag))
                    tagsFrequency[tag] = 0;
                tagsFrequency[tag]++;
                MaxFrequency = Math.Max(MaxFrequency, tagsFrequency[tag]);
            }
        }

        public Font GetTagFont(string tag)
        {
            double frequencyRatio = ((double)GetFrequency(tag) + 1) / (MaxFrequency + 1);
            // +1 for unknown tags, now it's size negligible, but not zero

            var fontSize = MaxFontEmSize * Math.Pow(frequencyRatio, FontSizeTuner);
            return FontCreator((float)fontSize);
        }

        public Size MeasureTag(string tag, Graphics graphics)
        {
            var currentFont = GetTagFont(tag);
            return (Size)graphics.MeasureString(tag, currentFont);
        }

        private int GetFrequency(string tag)
        {
            return tagsFrequency.ContainsKey(tag) ? tagsFrequency[tag] : 0;
        }
    }
}