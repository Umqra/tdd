using System.Collections.Generic;
using System.Linq;

namespace TagsCloudCore.Tags.Preparers
{
    public class NormalizeTagsTransformer : ITagsPreparer
    {
        public IEnumerable<string> PrepareTags(IEnumerable<string> tags)
        {
            return tags.Select(tag => tag.ToLower());
        }
    }
}