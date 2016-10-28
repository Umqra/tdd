using System.Collections.Generic;
using System.Drawing;

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