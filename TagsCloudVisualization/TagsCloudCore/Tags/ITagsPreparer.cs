using System.Collections.Generic;

namespace TagsCloudCore.Tags
{
    public interface ITagsPreparer
    {
        IEnumerable<string> PrepareTags(IEnumerable<string> lines);
    }
}