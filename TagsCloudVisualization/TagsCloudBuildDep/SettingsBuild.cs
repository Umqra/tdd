using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace TagsCloudBuildDep
{
    public class SettingsBuild : Module
    {
        private IBuildSettings BuildSettings { get; }
        public SettingsBuild(IBuildSettings settings)
        {
            BuildSettings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(BuildSettings).AsImplementedInterfaces();
        }
    }
}
