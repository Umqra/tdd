using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ResultOf;
using TagsCloudBuildDep;
using TagsCloudCli.Errors;
using TagsCloudCore.Layout;
using TagsCloudCore.Tags.Preparers;
using Point = Geometry.Point;

namespace TagsCloudCli
{
    public partial class CliOptions : ManagableCliOptions, IBuildSettings
    {
        public static readonly Dictionary<string, Func<Point, ITagsCloudLayouter>> LayouterNames =
            new Dictionary<string, Func<Point, ITagsCloudLayouter>>
            {
                {"random", center => new DenseRandomTagsCloudLayouter(center)},
                {"sparse", center => new SparseRandomTagsCloudLayouter(center)}
            };
        
        public Color ForegroundColor { get; private set; }

        public ITagsCloudLayouter Layouter { get; private set; }

        public FontFamily FontFamily { get; set; }

        public Color BackgroundColor { get; private set; }

        public string ConfigFilename { get; set; }

        public Color BrightColor
        {
            get { throw new NotImplementedException(); }
        }

        public Color FadeColor
        {
            get { throw new NotImplementedException(); }
        }

        public Color ShadeColor
        {
            get { throw new NotImplementedException(); }
        }

        public Color Color => ForegroundColor;

        int IFirstTagsTakerSettings.TagsCount
        {
            get
            {
                if (MaxTagsCount != null) return MaxTagsCount.Value;
                throw new ArgumentException();
            }
        }

        public float MaxFontEmSize => MaximumFontSize.Value;

        public Result<None> Initialize()
        {
            var configInitialization =
                InitializeNotNullOption(ConfigFilename, nameof(ConfigFilename))
                    .Then(InitializeFileForReading)
                    .Then(LoadConfig);
            if (!configInitialization.IsSuccess && !configInitialization.Error.Is<NullOptionError>())
                return configInitialization
                    .RefineError(error => new ConfigLoadError($"Error while load config from file {ConfigFilename}", error));
            
            return 
                InitializeNotNullOption(InputFilename, nameof(InputFilename)).Then(InitializeFileForReading)
                .PassErrorThrough(InitializeFileForWriting(OutputFilename))
                .PassErrorThrough(InitializeColor(BackgroundColorName).Then(color => BackgroundColor = color))
                .PassErrorThrough(InitializeColor(ForegroundColorName).Then(color => ForegroundColor = color))
                .PassErrorThrough(InitializeFontFamily(FontFamilyName).Then(fontFamily => FontFamily = fontFamily))
                .PassErrorThrough(InitializeLayouter(LayouterName).Then(layouter => Layouter = layouter))
                .RefineError(error => new Error("Fail while initialization CLI options", error))
                .AsNoneResult();
        }
    }
}