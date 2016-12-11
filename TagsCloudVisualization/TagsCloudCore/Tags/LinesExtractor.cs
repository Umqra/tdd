using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudCore.Tags
{
    public class LinesExtractor : ILinesExtractor
    {
        public IEnumerable<string> Extract(StreamReader stream)
        {
            while (!stream.EndOfStream)
            {
                yield return stream.ReadLine();
            }
        }
    }
}