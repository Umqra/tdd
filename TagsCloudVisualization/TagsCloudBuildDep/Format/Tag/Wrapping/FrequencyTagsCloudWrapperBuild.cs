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
            // CR (krait): Почему просто не зарегистрировать инстанс? Разве MaximumFontSize может поменяться в процессе работы приложения? 
            // А если вдруг может, то надо срочно запретить: переконфигурация DI-контейнера в рантайме ведёт к отвратительным и неуловимым багам.
            //TODO: context? again?
            builder.Register(context => context.Resolve<FrequencyTagsCloudWrapperFactory>()(MaximumFontSize))
                .As<ITagsWrapper>();
        }
    }
}