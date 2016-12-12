using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Features.Metadata;
using TagsCloudCore.Tags;

namespace TagsCloudBuildDep.Tags
{
    public class PreparersBuild : Module
    {
        private const string OrderField = "order";

        private IEnumerable<ITagsPreparer> Preparers { get; }

        public PreparersBuild(IEnumerable<ITagsPreparer> preparers)
        {
            Preparers = preparers;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var preparersList = Preparers.ToList();
            for (int i = 0; i < preparersList.Count; i++)
            {
                builder.RegisterInstance(preparersList[i]).As<ITagsPreparer>().WithMetadata(OrderField, i);
            }
            builder.Register(c => c
                    .Resolve<IEnumerable<Meta<ITagsPreparer>>>()
                    .OrderBy(i => i.Metadata[OrderField])
                    .Select(i => i.Value))
                .As<IEnumerable<ITagsPreparer>>();
        }
    }
}