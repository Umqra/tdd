﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using Fclp;
using TagsCloudCore.Visualization;

namespace TagsCloudCli
{
    internal class EntryPoint
    {
        internal static void Main(string[] args)
        {
            try
            {
                RunCli(args);
            }
            catch (Exception exception)
            {
                var currentException = exception;
                while (currentException != null)
                {
                    Console.WriteLine(currentException);
                    currentException = currentException.InnerException;
                }
            }
        }

        private static void RunCli(string[] args)
        {
            var parser = ConfigureCommandParser();
            var parsingStatus = parser.Parse(args);
            if (ShouldTerminateCli(parsingStatus, parser))
                return;

            var options = parser.Object;
            try
            {
                options.Initialize();
            }
            catch (Exception e)
            {
                throw new FormatException("Error occured while parsing CLI parameters", e);
            }

            var container = new AppCompositionRoot().BuildDependencies(options);
            var visualizator = container.Resolve<ITagsCloudVisualizator>();

            if (options.OutputFilename != null)
                ProcessBitmapImage(options, visualizator);
            else
                ProcessWinFormsApplication(options, visualizator);
        }

        private static void ProcessWinFormsApplication(CliOptions options, ITagsCloudVisualizator visualizator)
        {
            Application.Run(
                new TagsCloudDisplayForm(visualizator, options.Width, options.Height)
            );
        }

        private static void ProcessBitmapImage(CliOptions options, ITagsCloudVisualizator visualizator)
        {
            var image = new Bitmap(options.Width, options.Height);
            var graphics = Graphics.FromImage(image);
            visualizator.CreateTagsCloud(graphics);
            image.Save(options.OutputFilename);
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

        private static FluentCommandLineParser<CliOptions> ConfigureCommandParser()
        {
            var parser = new FluentCommandLineParser<CliOptions>();
            parser.Setup(arg => arg.InputFilename)
                .As('i', "input")
                .WithDescription("Filename with input text. You can use texts from examples/ folder.")
                .Required();
            parser.Setup(arg => arg.OutputFilename)
                .As('o', "output")
                .WithDescription(
                    "Filename for output image. You can skip this parameter - than cloud will be generated in WF Application.");

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

            parser.Setup(arg => arg.BackgroundColorName)
                .As("bc")
                .WithDescription(
                    "Background image color\nYou can use common names for colors or hex codes. For example: --fc orange, --fc #abc123.")
                .SetDefault("white");
            parser.Setup(arg => arg.ForegroundColorName)
                .As("fc")
                .WithDescription(
                    "Text color\nYou can use common names for colors or hex codes. For example: --fc orange, --fc #abc123.")
                .SetDefault("black");
            parser.Setup(arg => arg.LayouterName)
                .As('l', "layouter")
                .WithDescription(
                    $"Choose implementation of layouter from list: {string.Join(",", CliOptions.LayouterNames.Keys)}.")
                .SetDefault("sparse");
            parser.Setup(arg => arg.MaxTagsCount)
                .As('m', "max-tags")
                .WithDescription("This parameter limits amount of tags in the cloud");

            parser.SetupHelp("help", "?")
                .WithHeader("Help for 'Command line tags cloud generator application'")
                .WithCustomFormatter(new CustomCommandLineOptionFormatter())
                .Callback(text => Console.WriteLine(text));
            return parser;
        }
    }
}