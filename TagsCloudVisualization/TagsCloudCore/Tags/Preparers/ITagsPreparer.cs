using System.Collections.Generic;

namespace TagsCloudCore.Tags.Preparers
{
    public interface ITagsPreparer
    {
        IEnumerable<string> PrepareTags(IEnumerable<string> tags);
    }
}
