using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace TagsCloudCli.Extensions
{
    public static class YamlExtensions
    {
        public static IEnumerable<Tuple<string, string>> EnumerateYamlMembers<T>()
        {
            foreach (var property in typeof(T).GetProperties()
                .Where(prop => prop.IsDefined(typeof(YamlMemberAttribute), false)))
            {
                var yamlMember = property.GetCustomAttribute<YamlMemberAttribute>();
                if (yamlMember.Alias != null)
                    yield return Tuple.Create(yamlMember.Alias, property.Name);
                else
                    yield return Tuple.Create(property.Name, property.Name);
            }
        }
    }
}
