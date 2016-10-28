using System.Drawing;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCore.Format
{
    public class TagsDecoratorCombinator : ITagsDecorator
    {
        public ITagsDecorator[] Decorators { get; set; }

        public TagsDecoratorCombinator(params ITagsDecorator[] decorators)
        {
            Decorators = decorators;
        }

        public void DecorateTag(string tag, Font tagFont, Rectangle tagPlace, Graphics graphics)
        {
            foreach (var drawer in Decorators)
                drawer.DecorateTag(tag, tagFont, tagPlace, graphics);
        }
    }
}