using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudCore.Tags
{
    public class TagsExtractor : ITagsExtractor
    {
        private IEnumerable<string> SplitLineByTokens(string line)
        {
            return Regex.Split(line, @"\b");
        }

        public IEnumerable<string> ExtractTags(IEnumerable<string> lines)
        {
            return lines.SelectMany(SplitLineByTokens);
        }
    }
}