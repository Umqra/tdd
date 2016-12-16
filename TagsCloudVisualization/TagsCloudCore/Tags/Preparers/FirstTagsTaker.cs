using System.Collections.Generic;
using System.Linq;

namespace TagsCloudCore.Tags.Preparers
{
    public class FirstTagsTaker : ITagsPreparer
    {
        public int TagsCount { get; set; }

        public FirstTagsTaker(int tagsCount)
        {
            TagsCount = tagsCount;
        }

        public IEnumerable<string> PrepareTags(IEnumerable<string> tags)
        {
            return tags.Take(TagsCount);
        }
    }
}