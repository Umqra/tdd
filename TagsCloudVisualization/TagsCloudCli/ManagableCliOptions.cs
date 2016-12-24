using YamlDotNet.Serialization;

namespace TagsCloudCli
{
    public class ManagableCliOptions
    {
        [YamlMember(Alias = "output")]
        public string OutputFilename { get; set; }

        [YamlMember(Alias = "width")]
        public int? Width { get; set; }
        [YamlMember(Alias = "height")]
        public int? Height { get; set; }

        [YamlMember(Alias = "max-font-size")]
        public int? MaximumFontSize { get; set; }
        [YamlMember(Alias = "max-tags-count")]
        public int? MaxTagsCount { get; set; }

        [YamlMember(Alias = "font-family")]
        public string FontFamilyName { get; set; }

        [YamlMember(Alias = "background-color")]
        public string BackgroundColorName { get; set; }
        [YamlMember(Alias = "foreground-color")]
        public string ForegroundColorName { get; set; }

        [YamlMember(Alias = "layouter")]
        public string LayouterName { get; set; }

        [YamlMember(Alias = "input")]
        public string InputFilename { get; set; }
    }
}