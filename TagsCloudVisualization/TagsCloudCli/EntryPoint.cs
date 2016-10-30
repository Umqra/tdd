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

namespace TagsCloudCli
{
    internal class EntryPoint
    {
        // Nit: As we discussed, this is totally acceptable per se,
        // but in Kuntur styleguide lowerCamelCase is preferred for private static readonly
        private static readonly Dictionary<string, Func<Point, ITagsCloudLayouter>> LayouterNames =
            new Dictionary<string, Func<Point, ITagsCloudLayouter>>
            {
                {"random", center => new DenseRandomTagsCloudLayouter(center)},
                {"sparse", center => new SparseRandomTagsCloudLayouter(center)}
            };

        internal static void Main(string[] args)
        {
            var parser = ConfigureCommandParser();
            var parsingStatus = parser.Parse(args);
            if (ShouldTerminateCli(parsingStatus, parser))
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
                visualizator.CreateTagsCloud(tags.Distinct(), graphics);
                image.Save(options.OutputFilename);
            }
            else
            {
                Application.Run(new TagsCloudDisplayForm(visualizator, tags.Distinct().ToList(), 
                    options.Width, options.Height));
            }
        }

        private static bool ShouldTerminateCli(ICommandLineParserResult parsingStatus,
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

        private static ITagsCloudLayouter GetLayouterByNameWithFixedCenter(string name, Point center)
        {
            if (LayouterNames.ContainsKey(name))
                return LayouterNames[name](center);
            throw new ArgumentException("Unknown layouter name");
        }

        private static FluentCommandLineParser<CliOptions> ConfigureCommandParser()
        {
            var parser = new FluentCommandLineParser<CliOptions>();
            parser.Setup(arg => arg.InputFilename)
                .As('i', "input")
                .WithDescription("Filename with input text. You can use texts from examples/ folder.")
                .Required();

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
