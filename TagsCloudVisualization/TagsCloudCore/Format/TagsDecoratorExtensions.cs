namespace TagsCloudCore.Format
{
    public static class TagsDecoratorExtensions
    {
        public static ITagsDecorator With(this ITagsDecorator self, ITagsDecorator other)
        {
            return new TagsDecoratorCombinator(self, other);
        }
    }
}
