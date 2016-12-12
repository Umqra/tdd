using Autofac;
using TagsCloudCore.Visualization;

namespace TagsCloudBuildDep
{
    public class TagsCloudVisualizatorBuild : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VisualizatorConfiguration>().AsSelf();
            builder.RegisterType<TagsCloudVisualizator>().As<ITagsCloudVisualizator>();
        }
    }
}