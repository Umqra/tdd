using System;
using System.Drawing;
using System.IO;
using System.Linq;
using ResultOf;
using TagsCloudCli.Errors;
using TagsCloudCli.Extensions;
using TagsCloudCore.Layout;
using Point = Geometry.Point;

namespace TagsCloudCli
{
    public partial class CliOptions
    {
        private Result<string> InitializeInputFile(string filename)
        {
            return FileExtensions.HaveReadAccess(filename)
                .RefineError(error => new ReadInputFileError($"Can't read from input file {filename}", error));
        }

        private Result<string> InitializeOutputFile(string filename)
        {
            if (filename == null)
                return filename.AsResult();
            return FileExtensions.HaveWriteAccess(filename).RefineError(error =>
                    new WriteOutputFileError($"Can't write to output file {filename}", error));
        }

        private Result<Color> InitializeColor(string colorRepresentation)
        {
            return ColorExtensions.ParseColor(colorRepresentation);
        }

        private Result<ITagsCloudLayouter> InitializeLayouter(string layouterName)
        {
            if (!LayouterNames.ContainsKey(layouterName))
                return Result.Fail<ITagsCloudLayouter>(
                    new InvalidLayouterError(
                        $"Unknown layouter name {layouterName}. " +
                        $"You can use one from the list: {string.Join(", ", LayouterNames.Keys)}"));
            return LayouterNames[layouterName](new Point(Width / 2, Height / 2)).AsResult();
        }

        private Result<FontFamily> InitializeFontFamily(string fontFamilyName)
        {
            var normalizedName = fontFamilyName.ToLower().Trim();
            foreach (var font in FontFamily.Families)
            {
                if (font.Name.ToLower().Trim() == normalizedName)
                    return font.AsResult();
            }
            return Result.Fail<FontFamily>(
                new UnknownFontError($"Unknown font family {fontFamilyName}. " +
                                     $"Use one font from next list: {string.Join(",", FontFamily.Families.Select(f => f.Name))}"));
        }
    }
}
