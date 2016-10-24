﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TagsCloudCore
{
    public class VisualizatorConfiguration
    {
        public Func<ICloudLayouter> Layouter { get; set; }
        public Func<ICloudFormatter> Formatter { get; set; }
    }

    public class TagsCloudVisualizator
    {
        public VisualizatorConfiguration Configuration { get; }

        public TagsCloudVisualizator(VisualizatorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void CreateTagsCloud(IEnumerable<string> tags, Graphics graphics)
        {
            var layouter = Configuration.Layouter();
            var formatter = Configuration.Formatter();
            foreach (var tag in tags)
            {
                var tagBoundingBox = formatter.MeasureString(tag, graphics);
                var tagPlace = layouter.PutNextRectangle(tagBoundingBox);
                formatter.PutTag(tag, tagPlace, graphics);
            }
        }
    }
}