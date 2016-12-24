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
    public partial class CliOptions : IBuildSettings
    {
        public static readonly Dictionary<string, Func<Point, ITagsCloudLayouter>> LayouterNames =
            new Dictionary<string, Func<Point, ITagsCloudLayouter>>
            {
                {"random", center => new DenseRandomTagsCloudLayouter(center)},
                {"sparse", center => new SparseRandomTagsCloudLayouter(center)}
            };

        public string OutputFilename { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int MaximumFontSize { get; set; }
        public int? MaxTagsCount { get; set; }

        public string FontFamilyName { get; set; }

        public string BackgroundColorName { get; set; }

        public string ForegroundColorName { get; set; }

        public string LayouterName { get; set; }

        public Color ForegroundColor { get; private set; }

        public ITagsCloudLayouter Layouter { get; private set; }

        public FontFamily FontFamily { get; set; }

        public string InputFilename { get; set; }

        public Color BackgroundColor { get; private set; }

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

        public float MaxFontEmSize => MaximumFontSize;

        public Result<None> Initialize()
        {
            return 
                InitializeInputFile(InputFilename)
                .PassErrorThrough(InitializeOutputFile(OutputFilename))
                .PassErrorThrough(InitializeColor(BackgroundColorName).Then(color => BackgroundColor = color))
                .PassErrorThrough(InitializeColor(ForegroundColorName).Then(color => ForegroundColor = color))
                .PassErrorThrough(InitializeFontFamily(FontFamilyName).Then(fontFamily => FontFamily = fontFamily))
                .PassErrorThrough(InitializeLayouter(LayouterName).Then(layouter => Layouter = layouter))
                .RefineError(error => new Error("Fail while initialization CLI options", error))
                .AsNoneResult();
        }
    }
}