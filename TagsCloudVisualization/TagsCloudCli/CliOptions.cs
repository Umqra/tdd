using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using TagsCloudBuildDep;
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

        [Initialize(MethodName = nameof(InitializeOutputFile))]
        public string OutputFilename { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int MaximumFontSize { get; set; }
        public int? MaxTagsCount { get; set; }

        [Initialize(
             MethodName = nameof(InitializeFontFamily),
             BackingFieldName = nameof(FontFamily))
        ]
        public string FontFamilyName { get; set; }

        [Initialize(
             MethodName = nameof(InitializeColor),
             BackingFieldName = nameof(BackgroundColor)
         )]
        public string BackgroundColorName { get; set; }

        [Initialize(
             MethodName = nameof(InitializeColor),
             BackingFieldName = nameof(ForegroundColor)
         )]
        public string ForegroundColorName { get; set; }

        [Initialize(
             MethodName = nameof(InitializeLayouter),
             BackingFieldName = nameof(Layouter)
         )]
        public string LayouterName { get; set; }

        public Color ForegroundColor { get; private set; }

        public ITagsCloudLayouter Layouter { get; private set; }

        public FontFamily FontFamily { get; set; }

        [Initialize(MethodName = nameof(InitializeInputFile))]
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

        public void Initialize()
        {
            try
            {
                foreach (var field in typeof(CliOptions).GetProperties())
                {
                    var initializeAttribute =
                        (InitializeAttribute)
                        field.GetCustomAttributes(typeof(InitializeAttribute), true).FirstOrDefault();

                    if (initializeAttribute == null)
                        continue;
                    var initializeMethod = typeof(CliOptions).GetMethod(initializeAttribute.MethodName,
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    var propertyValue = field.GetValue(this);
                    var initializedValue = initializeMethod.Invoke(this, new[] {propertyValue});
                    if (initializeAttribute.BackingFieldName != null)
                    {
                        var backingProperty = typeof(CliOptions).GetProperty(initializeAttribute.BackingFieldName);
                        backingProperty.SetValue(this, initializedValue);
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }
        }
    }
}