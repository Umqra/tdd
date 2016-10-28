using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Fclp;
using TagsCloudCore;
using TagsCloudCore.Format;
using TagsCloudCore.Layout;
using TagsCloudCore.Visualization;
using Point = Geometry.Point;

// CR: Generator seems like a bad name for the application. Imagine calling tagscloud.generator.
// What it's going to do? Generate a picture? Generate a text? Start a web ui for generating pictures?
// Not very clear. It's a command-line interface, right? What about TagsCloud.Cli? Then it's clear that
// this application is a CLI for library/product TagsCloud. At least it won't start the web-server by default :)
// You can come up with something even better.

namespace TagsCloudCli
{
    // CR: Mark classes internal explicitly (uniformity with public classes & better readability)
    // CR: This is not Generator, this is EntryPoint/Program.
    internal class EntryPoint
    {
        // CR: In C# you don't need forward declaration, used functions usually placed below their usages
        // CR: 30+ lines, seems like a large function
        internal static void Main(string[] args)
        {
            var parser = ConfigureCommandParser();
            // CR: result - bad name, too ambiguous in the context
            var parsingStatus = parser.Parse(args);
            if (ShouldTerminateCLI(parsingStatus, parser))
                return;

            var options = parser.Object;

            List<string> tags;
            ITagsCloudVisualizator visualizator;
            try
            {
                tags = AlertIfAnyErrorOccured(() => new TagsExtractor().ExtractFromFile(options.InputFilename).ToList(),
                    $"Error while reading file {options.InputFilename} occured");
                visualizator = AlertIfAnyErrorOccured(() => ConfigureVisualizator(options, tags),
                    "Error while parsing visualizator parameters occured");
            }
            catch (FormatException)
            {
                parser.HelpOption.ShowHelp(parser.Options); 
                return;
            }


            if (options.OutputFilename != null)
            {
                var image = new Bitmap(options.Width, options.Height);
                var graphics = Graphics.FromImage(image);
                // CR: I see some duplications, do you?
                visualizator.CreateTagsCloud(tags.Distinct(), graphics);
                image.Save(options.OutputFilename);
            }
            else
            {
                Application.Run(new TagsCloudDisplayForm(visualizator, tags.Distinct().ToList(), 
                    options.Width, options.Height));
            }
        }

        private static bool ShouldTerminateCLI(ICommandLineParserResult parsingStatus,
            FluentCommandLineParser<CliOptions> parser)
        {
            if (parsingStatus.HelpCalled) return true;
            if (parsingStatus.HasErrors)
            {
                Console.WriteLine(parsingStatus.ErrorText);
                foreach (var error in parsingStatus.Errors)
                    parser.HelpOption.ShowHelp(new[] {error.Option});
                return true;
            }
            return false;
        }

        private static T AlertIfAnyErrorOccured<T>(Func<T> getter, string errorMessagePrefix)
        {
            try
            {
                return getter();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{errorMessagePrefix}: {exception.Message}");
                throw new FormatException();
            }
        }

        // CR: Mark methods private explicitly
        // CR: It's not GetColor, it's ParseColor
        // CR: What if color is not correct? It'll return uninitialized color. Is it OK?
        private static Color ParseColor(string colorRepresentation)
        {
            if (colorRepresentation[0] == '#')
            {
                if (colorRepresentation.Length != 7 || !colorRepresentation.Skip(1).All(c => c.IsHex()))
                    throw new ArgumentException($"Invalid hex color code {colorRepresentation}");
                return ColorTranslator.FromHtml(colorRepresentation);
            }
            var parsedColor = Color.FromName(colorRepresentation);
            if (!parsedColor.IsKnownColor)
                throw new ArgumentException($"Unknown color representation: {colorRepresentation}");
            return parsedColor;
        }

        private static readonly Dictionary<string, Func<Point, ITagsCloudLayouter>> LayouterNames =
            new Dictionary<string, Func<Point, ITagsCloudLayouter>>
            {
                {"random", center => new DenseRandomTagsCloudLayouter(center)},
                {"sparse", center => new SparseRandomTagsCloudLayouter(center)}
            };

        // CR: Don't use clousures and lambdas when it's not needed
        // Why do you need lambda here?
        private static ITagsCloudLayouter GetLayouterByNameWithFixedCenter(string name, Point center)
        {
            if (name == "random")
                return new DenseRandomTagsCloudLayouter(center);
            if (name == "sparse")
                return new SparseRandomTagsCloudLayouter(center);
            throw new ArgumentException("Unknown layouter name");
        }

        private static FluentCommandLineParser<CliOptions> ConfigureCommandParser()
        {
            var parser = new FluentCommandLineParser<CliOptions>();
            parser.Setup(arg => arg.InputFilename)
                .As('i', "input")
                .WithDescription("Filename with input text. You can use texts from examples/ folder.")
                .Required();
                // CR: Bad practice, better make it required parameter and add an example.
                // For example, make folder 'examples', move 'text.txt' to 'examples/sample.txt'
                // And set parameter in VS to use this file, so it's part of developer's setup, not output package

            parser.Setup(arg => arg.OutputFilename)
                .As('o', "output")
                .WithDescription("Filename for output image. You can skip this parameter - than cloud will be generated in WF Application.");

            parser.Setup(arg => arg.Width)
                .As('w', "width")
                .WithDescription("Width of generated image in pixels.")
                .SetDefault(800);
            parser.Setup(arg => arg.Height)
                .As('h', "height")
                .WithDescription("Height of generated image in pixels.")
                .SetDefault(800);
            parser.Setup(arg => arg.MaximumFontSize)
                .As('f', "font")
                .WithDescription("Maximum font size in em units.")
                .SetDefault(40);
            parser.Setup(arg => arg.BackgroundColor)
                .As("bc")
                .WithDescription("Background image color\nYou can use common names for colors or hex codes. For example: --fc orange, --fc #abc123.")
                .SetDefault("white");
            parser.Setup(arg => arg.ForegroundColor)
                .As("fc")
                .WithDescription("Text color\nYou can use common names for colors or hex codes. For example: --fc orange, --fc #abc123.")
                .SetDefault("black");
            parser.Setup(arg => arg.LayouterName)
                .As('l', "layouter")
                // CR: What options do I have?
                .WithDescription($"Choose implementation of layouter from list: {string.Join(",", LayouterNames.Keys)}.")
                .SetDefault("sparse");

            parser.SetupHelp("help", "?")
                .WithHeader("Help for 'Command line tags cloud generator application'")
                .WithCustomFormatter(new CustomCommandLineOptionFormatter())
                .Callback(text => Console.WriteLine(text));
            return parser;
        }

        private static ITagsCloudVisualizator ConfigureVisualizator(CliOptions options, IEnumerable<string> tags)
        {
            var layouter = GetLayouterByNameWithFixedCenter(options.LayouterName,
                new Point(options.Width / 2.0, options.Height / 2.0));
            var wrapper = new FrequencyTagsCloudWrapper(FontFamily.GenericSerif, options.MaximumFontSize, tags);
            var decorator = new SolidColorTagsDecorator(ParseColor(options.ForegroundColor));
            // CR: What if there's no file?
            // CR: What's the benifit in having really long lines?
            return new TagsCloudVisualizator(
                new VisualizatorConfiguration
                {
                    Layouter = () => layouter,
                    Wrapper = () => wrapper,
                    Decorator = () => decorator
                },
                ParseColor(options.BackgroundColor)
            );
        }
    }
}
