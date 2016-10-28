using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace TagsCloudCore.Visualization
{
    public class TagsCloudVisualizator : ITagsCloudVisualizator
    {
        public VisualizatorConfiguration Configuration { get; }
        public Color BackgroundColor { get; }

        public TagsCloudVisualizator(VisualizatorConfiguration configuration, Color backgroundColor)
        {
            Configuration = configuration;
            BackgroundColor = backgroundColor;
        }

        public void CreateTagsCloud(IEnumerable<string> tags, Graphics graphics)
        {
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(BackgroundColor);
            var layouter = Configuration.Layouter();
            var wrapper = Configuration.Wrapper();
            var decorator = Configuration.Decorator();
            foreach (var tag in tags)
            {
                var tagBoundingBox = wrapper.MeasureTag(tag, graphics);
                var tagFont = wrapper.GetTagFont(tag);
                var tagPlace = layouter.PutNextRectangle(tagBoundingBox);
                decorator.DecorateTag(tag, tagFont, tagPlace, graphics);
            }
        }
    }
}