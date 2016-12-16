using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudCore.Tags.Preparers;

namespace TagsCloudCore.Tags
{
    public class TagsCreator : ITagsCreator
    {
        private string InputFilename { get; }
        private ILinesExtractor LinesExtractor { get; }
        private ITagsExtractor TagsExtractor { get; }
        private IEnumerable<ITagsPreparer> TagsPreparers { get; }
        public TagsCreator(string inputFilename, ILinesExtractor linesExtractor, ITagsExtractor tagsExtractor,
            IEnumerable<ITagsPreparer> tagsPreparers)
        {
            InputFilename = inputFilename;
            LinesExtractor = linesExtractor;
            TagsExtractor = tagsExtractor;
            TagsPreparers = tagsPreparers;
        }

        public IEnumerable<string> GetTags()
        {
            using (var stream = new StreamReader(File.OpenRead(InputFilename)))
            {
                var lines = LinesExtractor.ExtractLines(stream);
                var rawTags = TagsExtractor.ExtractTags(lines);
                return TagsPreparers
                        .Aggregate(rawTags, (current, preparer) => preparer.PrepareTags(current))
                        .ToList();
            }
        }
    }
}
