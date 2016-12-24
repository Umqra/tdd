using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudCore.Tags.Preparers;

namespace TagsCloudCore.Tags
{
    public class TagsCreator : ITagsCreator
    {
        private ITagsCreatorSettings Settings { get; }
        private ILinesExtractor LinesExtractor { get; }
        private ITagsExtractor TagsExtractor { get; }
        private IEnumerable<ITagsPreparer> TagsPreparers { get; }

        public TagsCreator(ITagsCreatorSettings settings, ILinesExtractor linesExtractor, ITagsExtractor tagsExtractor,
            IEnumerable<ITagsPreparer> tagsPreparers)
        {
            Settings = settings;
            LinesExtractor = linesExtractor;
            TagsExtractor = tagsExtractor;
            TagsPreparers = tagsPreparers;
        }

        public IEnumerable<string> GetTags()
        {
            using (var stream = File.OpenRead(Settings.InputFilename))
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