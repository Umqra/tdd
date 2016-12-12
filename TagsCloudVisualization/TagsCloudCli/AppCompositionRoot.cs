using System;
using System.Collections.Generic;
using System.Drawing;
using Autofac;
using TagsCloudBuildDep;
using TagsCloudBuildDep.Format.Tag.Wrapping;
using TagsCloudBuildDep.Tags;
using TagsCloudCore.Format.Background;
using TagsCloudCore.Format.Tag.Decorating;
using TagsCloudCore.Layout;
using TagsCloudCore.Tags;

namespace TagsCloudCli
{
    public class AppCompositionRoot
    {
        public IContainer BuildDependencies(CliOptions options)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new TagsCreatorBuild(options.InputFilename));
            builder.RegisterModule(new StrictOrderEnumerationBuild<ITagsPreparer>(GetPreparers(options)));

            builder.RegisterInstance<Func<float, Font>>(size => new Font(FontFamily.GenericSerif, size))
                .As<Func<float, Font>>();
            builder.RegisterInstance(options.Layouter).As<ITagsCloudLayouter>();

            builder.RegisterModule(new StrictOrderEnumerationBuild<ITagsDecorator>(GetDecorators(options)));
            builder.RegisterModule(
                new StrictOrderEnumerationBuild<IBackgroundDecorator>(GetBackgroundDecorators(options)));

            builder.RegisterModule(new FrequencyTagsCloudWrapperBuild(options.MaximumFontSize));
            builder.RegisterModule(new TagsCloudVisualizatorBuild());

            return builder.Build();
        }

        private IEnumerable<ITagsPreparer> GetPreparers(CliOptions options)
        {
            yield return new NormalizeTagsTransformer();
            yield return new LetterTagsFilter();
            yield return new StopWordsFilter();
            if (options.MaxTagsCount.HasValue)
                yield return new FirstTagsTaker(options.MaxTagsCount.Value);
        }

        private IEnumerable<ITagsDecorator> GetDecorators(CliOptions options)
        {
            yield return new SolidColorTagsDecorator(options.ForegroundColor);
        }

        private IEnumerable<IBackgroundDecorator> GetBackgroundDecorators(CliOptions options)
        {
            yield return new SolidBackgroundDecorator(options.BackgroundColor);
        }
    }
}