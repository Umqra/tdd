using System.Collections.Generic;
using System.Linq;
using Autofac;
using TagsCloudCore.Tags;

namespace TagsCloudBuildDep.Tags
{
    public class TagsCreatorBuild : Module
    {
        public delegate TagsCreator TagsCreatorFactory(string inputFilename);

        public string InputFilename { get; }

        public TagsCreatorBuild(string inputFilename)
        {
            InputFilename = inputFilename;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LinesExtractor>().As<ILinesExtractor>();
            builder.RegisterType<TagsExtractor>().As<ITagsExtractor>();

            builder.RegisterType<TagsCreator>();
            //TODO: think about this two lines
            builder.Register(context => context.Resolve<TagsCreatorFactory>()(InputFilename)).As<ITagsCreator>();
            builder.Register(context => context.Resolve<ITagsCreator>().GetTags().ToList())
                .As<IEnumerable<string>>()
                .InstancePerLifetimeScope();
        }
    }
}