using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace TagsCloudCore.Visualization
{
    public class TagsCloudVisualizator : ITagsCloudVisualizator
    {
        public VisualizatorConfiguration Configuration { get; }
        public Color BackgroundColor { get; }
        public int? MaxTagsCount { get; set; }

        public TagsCloudVisualizator(VisualizatorConfiguration configuration, Color backgroundColor, int? maxTagsCount)
        {
            Configuration = configuration;
            BackgroundColor = backgroundColor;
            MaxTagsCount = maxTagsCount;
        }

        public void CreateTagsCloud(IEnumerable<string> tags, Graphics graphics)
        {
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(BackgroundColor);
            var layouter = Configuration.Layouter();
            var wrapper = Configuration.Wrapper();
            var decorator = Configuration.Decorator();
            if (MaxTagsCount.HasValue)
                tags = tags.Take(MaxTagsCount.Value);

            foreach (var tag in tags)
            {
                var tagSize = wrapper.MeasureTag(tag, graphics);
                var tagFont = wrapper.GetTagFont(tag);
                var tagPlace = layouter.PutNextRectangle(tagSize);
                decorator.DecorateTag(tag, tagFont, tagPlace, graphics);
            }
        }
    }
}