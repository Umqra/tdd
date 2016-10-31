using System;

namespace TagsCloudCli
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InitializeAttribute : Attribute
    {
        public string MethodName { get; set; }
        public string BackingFieldName { get; set; }
    }
}