using Autofac;
using TagsCloudCore.Format.Tag.Wrapping;

namespace TagsCloudBuildDep.Format.Tag.Wrapping
{
    public class FrequencyTagsCloudWrapperBuild : Module
    {
        public delegate FrequencyTagsCloudWrapper FrequencyTagsCloudWrapperFactory(float maxFontEmSize);
        public float MaximumFontSize { get; }

        public FrequencyTagsCloudWrapperBuild(float maximumFontSize)
        {
            MaximumFontSize = maximumFontSize;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FrequencyTagsCloudWrapper>();
            //TODO: context? again?
            builder.Register(context => context.Resolve<FrequencyTagsCloudWrapperFactory>()(MaximumFontSize))
                .As<ITagsWrapper>();
        }
    }
}