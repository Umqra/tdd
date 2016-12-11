using System.Collections.Generic;

namespace TagsCloudCore.Tags
{
    public interface ITagsExtractor
    {
        IEnumerable<string> ExtractTags(IEnumerable<string> lines);
    }
}