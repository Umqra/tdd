using System.Collections.Generic;
using System.Drawing;
using Autofac;
using TagsCloudBuildDep;
using TagsCloudBuildDep.Tags;
using TagsCloudCore.Format.Background;
using TagsCloudCore.Format.Tag;
using TagsCloudCore.Format.Tag.Decorating;
using TagsCloudCore.Format.Tag.Wrapping;
using TagsCloudCore.Layout;
using TagsCloudCore.Tags.Preparers;

namespace TagsCloudCli
{
    public class AppCompositionRoot
    {
        public IContainer BuildDependencies(CliOptions options)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new SettingsBuild(options));

            builder.RegisterModule(new TagsCreatorBuild(options.InputFilename));

            builder.RegisterModule(ConfigurePreparers(new StrictOrderEnumerationBuild<ITagsPreparer>(), options));
            builder.RegisterModule(ConfigureDecorators(new StrictOrderEnumerationBuild<ITagsDecorator>(), options));

            builder.RegisterType<FixedFamilyFontProvider>().As<IFontProvider>();
            builder.RegisterInstance(options.Layouter).As<ITagsCloudLayouter>();

            builder.RegisterModule(ConfigureBackgroundDecorators(new StrictOrderEnumerationBuild<IBackgroundDecorator>(), options));

            builder.RegisterType<FrequencyTagsCloudWrapper>().As<ITagsWrapper>();
            builder.RegisterModule(new TagsCloudVisualizatorBuild());

            return builder.Build();
        }

        private Module ConfigurePreparers(StrictOrderEnumerationBuild<ITagsPreparer> enumerationBuild, CliOptions options)
        {
            enumerationBuild.Register<NormalizeTagsTransformer>();
            enumerationBuild.Register<LetterTagsFilter>();
            enumerationBuild.Register<StemTagTransform>();
            enumerationBuild.Register<StopWordsFilter>();
            if (options.MaxTagsCount.HasValue)
                enumerationBuild.Register<FirstTagsTaker>();
            return enumerationBuild;
        }

        private StrictOrderEnumerationBuild<ITagsDecorator> ConfigureDecorators(
            StrictOrderEnumerationBuild<ITagsDecorator> enumerationBuild, CliOptions options)
        {
            return enumerationBuild.Register<SolidColorTagsDecorator>();
        }

        private StrictOrderEnumerationBuild<IBackgroundDecorator> ConfigureBackgroundDecorators(
            StrictOrderEnumerationBuild<IBackgroundDecorator> enumerationBuild, CliOptions options)
        {
            return enumerationBuild.Register<SolidBackgroundDecorator>();
        }
    }
}