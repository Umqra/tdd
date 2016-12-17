using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudCore.Tags
{
    public class TxtLinesExtractor : ILinesExtractor
    {
        public IEnumerable<string> ExtractLines(Stream stream)
        {
            var streamReader = new StreamReader(stream);
            while (!streamReader.EndOfStream)
            {
                yield return streamReader.ReadLine();
            }
        }
    }
}