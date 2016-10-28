using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Fclp;
using TagsCloudCore;
using TagsCloudCore.Format;
using TagsCloudCore.Layout;
using TagsCloudCore.Visualization;

// CR: Generator seems like a bad name for the application. Imagine calling tagscloud.generator.
// What it's going to do? Generate a picture? Generate a text? Start a web ui for generating pictures?
// Not very clear. It's a command-line interface, right? What about TagsCloud.Cli? Then it's clear that
// this application is a CLI for library/product TagsCloud. At least it won't start the web-server by default :)
// You can come up with something even better.
namespace TagsCloudGenerator
{
    // CR: One class = one file
    public class GeneratorOptions
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
    // CR: Mark classes internal explicitly (uniformity with public classes & better readability)
    // CR: This is not Generator, this is EntryPoint/Program.
    class TagsCloudGenerator
    {
        // CR: Mark methods private explicitly
        // CR: It's not GetColor, it's ParseColor
        // CR: What if color is not correct? It'll return uninitialized color. Is it OK?
        static Color GetColor(string colorRepresentation)
        {
            if (colorRepresentation[0] == '#')
                return ColorTranslator.FromHtml(colorRepresentation);
            return Color.FromName(colorRepresentation);
        }

        // CR: Don't use clousures and lambdas when it's not needed
        // Why do you need lambda here?
        static Func<Geometry.Point, ITagsCloudLayouter> GetLayouterByName(string name)
        {
            if (name == "random")
                return center => new DenseRandomTagsCloudLayouter(center);
            if (name == "sparse")
                return center => new SparseRandomTagsCloudLayouter(center);
            throw new ArgumentException("Unknown layouter name");
        }

        static FluentCommandLineParser<GeneratorOptions> ConfigureCommandParser()
        {
            var parser = new FluentCommandLineParser<GeneratorOptions>();
            parser.Setup(arg => arg.InputFilename)
                .As('i', "input")
                .WithDescription("Filename with input text")
                // CR: Bad practice, better make it required parameter and add an example.
                // For example, make folder 'examples', move 'text.txt' to 'examples/sample.txt'
                // And set parameter in VS to use this file, so it's part of developer's setup, not output package
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
            parser.Setup(arg => arg.LayouterName)
                .As('l', "layouter")
                // CR: What options do I have?
                .WithDescription("Choose implementation of layouter")
                .SetDefault("sparse");

            parser.SetupHelp("help", "?")
                .Callback(text => Console.WriteLine(text));
            return parser;
        }

        // CR: In C# you don't need forward declaration, used functions usually placed below their usages
        // CR: 30+ lines, seems like a large function
        static void Main(string[] args)
        {
            var parser = ConfigureCommandParser();
            // CR: result - bad name, too ambiguous in the context
            var result = parser.Parse(args);
            if (result.HelpCalled) return;
            if (result.HasErrors) return;

            var options = parser.Object;
            
            // CR: What if there's no file?
            // CR: What's the benifit in having really long lines?
            var tags = new TagsExtractor().ExtractFromFile(options.InputFilename);
            var visualizator = new TagsCloudVisualizator(
                new VisualizatorConfiguration
                {
                    Layouter =
                        () => GetLayouterByName(options.LayouterName)(new Geometry.Point(options.Width / 2.0,
                            options.Height / 2.0)),
                    Wrapper =
                        () => new FrequencyTagsCloudWrapper(FontFamily.GenericSerif, options.MaximumFontSize, tags),
                    Decorator = 
                        () => new SolidColorTagsDecorator(GetColor(options.ForegroundColor))
                }
            );
            if (options.OutputFilename != null)
            {
                var image = new Bitmap(options.Width, options.Height);
                var graphics = Graphics.FromImage(image);
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                // CR: I see some duplications, do you?
                graphics.Clear(GetColor(options.BackgroundColor));
                visualizator.CreateTagsCloud(tags.Distinct(), graphics);
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
