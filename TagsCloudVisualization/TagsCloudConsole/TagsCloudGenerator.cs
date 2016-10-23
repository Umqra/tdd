using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fclp;
using TagsCloudVisualization;

namespace TagsCloudConsole
{
    public class GeneratorOptions
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MaximumFontSize { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
    }
    class TagsCloudGenerator
    {
        static Color GetColor(string colorRepresentation)
        {
            if (colorRepresentation[0] == '#')
                return ColorTranslator.FromHtml(colorRepresentation);
            return Color.FromName(colorRepresentation);
        }

        static FluentCommandLineParser<GeneratorOptions> ConfigureCommandParser()
        {
            var parser = new FluentCommandLineParser<GeneratorOptions>();
            parser.Setup(arg => arg.InputFilename)
                .As('i', "input")
                .WithDescription("Filename with input text")
                .SetDefault("text.txt");

            parser.Setup(arg => arg.OutputFilename)
                .As('o', "output")
                .WithDescription("Filename for output image. You can skip this parameter - than cloud will be generated in WF Application");

            parser.Setup(arg => arg.Width)
                .As('w', "width")
                .SetDefault(800);
            parser.Setup(arg => arg.Height)
                .As('h', "height")
                .SetDefault(800);
            parser.Setup(arg => arg.MaximumFontSize)
                .As('f', "font")
                .WithDescription("Maximum font size in em units")
                .SetDefault(40);
            parser.Setup(arg => arg.BackgroundColor)
                .As("bc")
                .WithDescription("Background image color")
                .SetDefault("white");
            parser.Setup(arg => arg.ForegroundColor)
                .As("fc")
                .WithDescription("Text color")
                .SetDefault("black");

            parser.SetupHelp("help", "?")
                .Callback(text => Console.WriteLine(text));
            return parser;
        }
        
        static void Main(string[] args)
        {
            var parser = ConfigureCommandParser();
            var result = parser.Parse(args);
            if (result.HelpCalled) return;
            if (result.HasErrors) return;

            var options = parser.Object;
            
            var tags = new TagsExtractor().ExtractFromFile(options.InputFilename);
            var visualizator = new TagsCloudVisualizator(
                new VisualizatorConfiguration
                {
                    Layouter =
                        () =>
                            new CircularCloudLayouter(new Geometry.Point(options.Width / 2.0,
                                options.Height / 2.0)),
                    Formatter =
                        () =>
                            new FrequencyCloudFormatter(FontFamily.GenericSerif, options.MaximumFontSize,
                                new SolidBrush(GetColor(options.ForegroundColor)), tags)
                }
            );
            if (options.OutputFilename != null)
            {
                var image = new Bitmap(options.Width, options.Height);
                Graphics.FromImage(image).Clear(GetColor(options.BackgroundColor));
                visualizator.CreateTagsCloud(tags.Distinct(), Graphics.FromImage(image));
                image.Save(options.OutputFilename);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.Run(new CloudDisplayForm(visualizator, tags.Distinct().ToList(), options.Width, options.Height,
                    GetColor(options.BackgroundColor)));
            }
        }
    }
}
