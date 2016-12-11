using System.Collections.Generic;
using TagsCloudCore.Format.Background;
using TagsCloudCore.Format.Tag.Decorating;
using TagsCloudCore.Format.Tag.Wrapping;
using TagsCloudCore.Layout;

namespace TagsCloudCore.Visualization
{
    public class VisualizatorConfiguration
    {
        public ITagsCloudLayouter Layouter { get; set; }
        public ITagsWrapper Wrapper { get; set; }
        public IEnumerable<ITagsDecorator> Decorators { get; set; }
        public IEnumerable<IBackgroundDecorator> BackgroundDecorators { get; set; }
        public int? MaxTagsCount { get; set; }

        public VisualizatorConfiguration(
            ITagsCloudLayouter layouter,
            ITagsWrapper wrapper,
            IEnumerable<ITagsDecorator> decorators,
            IEnumerable<IBackgroundDecorator> backgroundDecorators,
            int? maxTagsCount
        )
        {
            Layouter = layouter;
            Wrapper = wrapper;
            Decorators = decorators;
            BackgroundDecorators = backgroundDecorators;
            MaxTagsCount = maxTagsCount;
        }
    }
}