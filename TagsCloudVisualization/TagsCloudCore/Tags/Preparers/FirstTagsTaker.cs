using System.Collections.Generic;
using System.Linq;

namespace TagsCloudCore.Tags.Preparers
{
    public class FirstTagsTaker : ITagsPreparer
    {
        private IFirstTagsTakerSettings Settings { get; }

        public FirstTagsTaker(IFirstTagsTakerSettings settings)
        {
            Settings = settings;
        }

        public IEnumerable<string> PrepareTags(IEnumerable<string> tags)
        {
            return tags.Take(Settings.TagsCount);
        }
    }
}