using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudCore.Format
{
    public static class TagsDrawerExtensions
    {
        public static ITagsDecorator With(this ITagsDecorator self, ITagsDecorator other)
        {
            return new TagsDecoratorCombinator(self, other);
        }
    }
}
