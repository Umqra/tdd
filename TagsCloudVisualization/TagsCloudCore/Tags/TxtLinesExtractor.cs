using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudCore.Tags
{
    public class TxtLinesExtractor : ILinesExtractor
    {
        public IEnumerable<string> ExtractLines(StreamReader stream)
        {
            while (!stream.EndOfStream)
            {
                yield return stream.ReadLine();
            }
        }
    }
}