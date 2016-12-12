using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Features.Metadata;

namespace TagsCloudBuildDep
{
    public class StrictOrderEnumerationBuild<T> : Module where T : class
    {
        private const string OrderField = "order";

        private IEnumerable<T> Enumeration { get; }

        public StrictOrderEnumerationBuild(IEnumerable<T> enumeration)
        {
            Enumeration = enumeration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var list = Enumeration.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                builder.RegisterInstance(list[i]).As<T>().WithMetadata(OrderField, i);
            }
            builder.Register(c => c
                    .Resolve<IEnumerable<Meta<T>>>()
                    .OrderBy(i => i.Metadata[OrderField])
                    .Select(i => i.Value))
                .As<IEnumerable<T>>();
        }
    }
}