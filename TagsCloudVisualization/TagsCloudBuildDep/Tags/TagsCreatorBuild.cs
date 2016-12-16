using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using TagsCloudCore.Tags;

namespace TagsCloudBuildDep.Tags
{
    public class TagsCreatorBuild : Module
    {
        public Dictionary<string, ILinesExtractor> LineExtractorByExtension = 
            new Dictionary<string, ILinesExtractor>
        {
            [".docx"] = new DocxLineExtractor(),
            [".txt"] = new TxtLinesExtractor(),
        };
        public ILinesExtractor DefaultExtractor = new TxtLinesExtractor();
        public delegate TagsCreator TagsCreatorFactory(string inputFilename);

        public string InputFilename { get; }

        public TagsCreatorBuild(string inputFilename)
        {
            InputFilename = inputFilename;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(GetLineExtractor()).As<ILinesExtractor>();
            builder.RegisterInstance(new TagsExtractor()).As<ITagsExtractor>();

            builder.RegisterType<TagsCreator>();
            builder.Register(context => context.Resolve<TagsCreatorFactory>()(InputFilename))
                .As<ITagsCreator>();
        }

        private ILinesExtractor GetLineExtractor()
        {
            var extension = Path.GetExtension(InputFilename);
            var extractor = DefaultExtractor;
            if (extension != null && LineExtractorByExtension.ContainsKey(extension))
                extractor = LineExtractorByExtension[extension];
            return extractor;
        }
    }
}