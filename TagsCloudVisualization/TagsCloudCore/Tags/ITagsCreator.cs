using System.Collections.Generic;

namespace TagsCloudCore.Tags
{
    public interface ITagsCreator
    {
        IEnumerable<string> GetTags();
    }
}