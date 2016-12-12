using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloudCore.Tags;

namespace TagsCloudCore.Visualization
{
    public class TagsCloudVisualizator : ITagsCloudVisualizator
    {
        public VisualizatorConfiguration Configuration { get; }
        public ITagsCreator TagsCreator { get; }

        public TagsCloudVisualizator(VisualizatorConfiguration configuration, ITagsCreator tagsCreator)
        {
            Configuration = configuration;
            TagsCreator = tagsCreator;
        }

        public void CreateTagsCloud(Graphics graphics)
        {
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            foreach (var backgroundDecorator in Configuration.BackgroundDecorators)
                backgroundDecorator.DecorateBackground(graphics);

            foreach (var tag in TagsCreator.GetTags())
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