using System.Collections.Generic;
using System.IO;

namespace TagsCloudCore.Tags
{
    public static class LinesExtractorExtensions
    {
        public static IEnumerable<string> ExtractFromFile(this ILinesExtractor extractor, string filename)
        {
            using (var stream = new StreamReader(File.Open(filename, FileMode.Open)))
            {
                return extractor.Extract(stream);
            }
        }
    }
}