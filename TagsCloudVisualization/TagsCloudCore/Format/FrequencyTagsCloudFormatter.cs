﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Rectangle = Geometry.Rectangle;
using Size = Geometry.Size;

namespace TagsCloudCore.Format
{
    public class FrequencyTagsCloudFormatter : ITagsCloudFormatter
    {
        public FontFamily FontFamily { get; set; }
        public float MaxFontSizeInEm { get; set; }
        public Brush Brush { get; set; }
        public Dictionary<string, int> TagsFrequency;
        // CR: Why underscore?
        private int MaxFrequency { get; }

        public FrequencyTagsCloudFormatter(FontFamily fontFamily, float maxFontSizeInEm, Brush brush, IEnumerable<string> tags)
        {
            FontFamily = fontFamily;
            MaxFontSizeInEm = maxFontSizeInEm;
            Brush = brush;

            TagsFrequency = new Dictionary<string, int>();
            foreach (var tag in tags)
            {
                if (!TagsFrequency.ContainsKey(tag))
                    TagsFrequency[tag] = 0;
                TagsFrequency[tag]++;
                MaxFrequency = Math.Max(MaxFrequency, TagsFrequency[tag]);
            }
        }

        private int GetFrequence(string tag)
        {
            return TagsFrequency.ContainsKey(tag) ? TagsFrequency[tag] : 0;
        }

        private Font GetTagFont(string tag)
        {
            var fontSize = MaxFontSizeInEm * Math.Pow(((double)GetFrequence(tag) + 1) / (MaxFrequency + 1), 0.7);
            return new Font(FontFamily, (float)fontSize);
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