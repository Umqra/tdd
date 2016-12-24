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

        [YamlMember(Alias = "max_font_size")]
        public int? MaximumFontSize { get; set; }
        [YamlMember(Alias = "max_tags_count")]
        public int? MaxTagsCount { get; set; }

        [YamlMember(Alias = "font_family")]
        public string FontFamilyName { get; set; }

        [YamlMember(Alias = "background_color")]
        public string BackgroundColorName { get; set; }
        [YamlMember(Alias = "foreground_color")]
        public string ForegroundColorName { get; set; }

        [YamlMember(Alias = "layouter")]
        public string LayouterName { get; set; }

        [YamlMember(Alias = "input")]
        public string InputFilename { get; set; }
    }
}