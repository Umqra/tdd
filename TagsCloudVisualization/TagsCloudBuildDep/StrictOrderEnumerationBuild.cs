using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Features.Metadata;

namespace TagsCloudBuildDep
{
    // CR (krait): Неужели autofac не позволяет решить эту задачу более изящно?
    public class StrictOrderEnumerationBuild<T> : Module where T : class
    {
        private const string OrderField = "order";

        private List<Action<ContainerBuilder>> RegistrationActions { get; }
        private int registrationId = 1;

        public StrictOrderEnumerationBuild()
        {
            RegistrationActions = new List<Action<ContainerBuilder>>();
        }

        public StrictOrderEnumerationBuild<T> Register<TNew>() where TNew: T
        {
            int currentId = registrationId++;
            RegistrationActions.Add(builder =>
            {
                builder.RegisterType<TNew>().As<T>().WithMetadata(OrderField, currentId);
            });
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            foreach (var action in RegistrationActions)
                action(builder);
            builder.Register(c => c
                    .Resolve<IEnumerable<Meta<T>>>()
                    .OrderBy(i => i.Metadata[OrderField])
                    .Select(i => i.Value))
                .As<IEnumerable<T>>();
        }
    }
}