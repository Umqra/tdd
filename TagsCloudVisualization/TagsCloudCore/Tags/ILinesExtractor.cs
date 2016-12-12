using System.Collections.Generic;
using System.IO;

namespace TagsCloudCore.Tags
{
    public interface ILinesExtractor
    {
        IEnumerable<string> ExtractLines(StreamReader stream);
    }
}