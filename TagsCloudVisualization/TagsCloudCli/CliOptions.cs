namespace TagsCloudCli
{
    public class CliOptions
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MaximumFontSize { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }

        public string LayouterName { get; set; }
    }
}