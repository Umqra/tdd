using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace TagsCloudCore.Visualization
{
    public class TagsCloudVisualizator : ITagsCloudVisualizator
    {
        public VisualizatorConfiguration Configuration { get; }

        public TagsCloudVisualizator(VisualizatorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void CreateTagsCloud(IEnumerable<string> tags, Graphics graphics)
        {
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            foreach (var backgroundDecorator in Configuration.BackgroundDecorators)
                backgroundDecorator.DecorateBackground(graphics);

            if (Configuration.MaxTagsCount.HasValue)
                tags = tags.Take(Configuration.MaxTagsCount.Value);

            foreach (var tag in tags)
            {
                var tagSize = Configuration.Wrapper.MeasureTag(tag, graphics);
                var tagFont = Configuration.Wrapper.GetTagFont(tag);
                var tagPlace = Configuration.Layouter.PutNextRectangle(tagSize);
                foreach (var decorator in Configuration.Decorators)
                    decorator.DecorateTag(tag, tagFont, tagPlace, graphics);
            }
        }
    }
}