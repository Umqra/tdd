using System;
using System.Drawing;
using System.IO;
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
            if (!FileExtensions.HaveReadAccess(filename))
                return Result.Fail<string>(new ReadInputFileError($"Can't read from input file {filename}"));
            return filename.AsResult();
        }

        private Result<string> InitializeOutputFile(string filename)
        {
            if (filename != null && !FileExtensions.HaveWriteAccess(filename))
                return Result.Fail<string>(new WriteOutputFileError($"Can't write to output file {filename}"));
            return filename.AsResult();
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
            fontFamilyName = fontFamilyName.ToLower().Trim();
            foreach (var font in FontFamily.Families)
            {
                if (font.Name.ToLower().Trim() == fontFamilyName)
                    return font.AsResult();
            }
            return FontFamily.GenericSerif.AsResult();
        }
    }
}
