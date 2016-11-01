using System;
using TagsCloudCore.Format;
using TagsCloudCore.Layout;

namespace TagsCloudCore.Visualization
{
    public class VisualizatorConfiguration
    {
        public Func<ITagsCloudLayouter> Layouter { get; set; }
        public Func<ITagsWrapper> Wrapper { get; set; }
        public Func<ITagsDecorator> Decorator { get; set; }
    }
}