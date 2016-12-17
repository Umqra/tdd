using Autofac;
using TagsCloudCore.Format.Tag.Wrapping;

namespace TagsCloudBuildDep.Format.Tag.Wrapping
{
    public class FrequencyTagsCloudWrapperBuild : Module
    {
        public float MaximumFontSize { get; }

        public FrequencyTagsCloudWrapperBuild(float maximumFontSize)
        {
            MaximumFontSize = maximumFontSize;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new FrequencyWrapperSettings(MaximumFontSize)).As<FrequencyWrapperSettings>();
            builder.RegisterType<FrequencyTagsCloudWrapper>().As<ITagsWrapper>();
        }
    }
}