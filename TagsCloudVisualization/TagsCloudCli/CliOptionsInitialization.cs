using System.Drawing;
using System.IO;
using System.Linq;
using ResultOf;
using TagsCloudCli.Errors;
using TagsCloudCli.Extensions;
using TagsCloudCore.Layout;
using Point = Geometry.Point;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TagsCloudCli
{
    public partial class CliOptions
    {
        private void FillFieldsWithOptions(ManagableCliOptions options)
        {
            foreach (var property in typeof(ManagableCliOptions).GetProperties()
                .Where(prop => prop.IsDefined(typeof(YamlMemberAttribute), false)))
            {
                var newValue = property.GetValue(options);
                if (newValue != null)
                    property.SetValue(this, newValue);
            }
        }
        private Result<None> LoadConfig(string configFilename)
        {
            var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new HyphenatedNamingConvention()).Build();

            return Result.FromFunction(() =>
                {
                    using (var stream = new StreamReader(File.OpenRead(ConfigFilename)))
                        return deserializer.Deserialize<ManagableCliOptions>(stream);
                })
                .Then(FillFieldsWithOptions)
                .AsNoneResult();
        }

        private Result<T> InitializeNotNullOption<T>(T value, string optionName)
        {
            return value == null
                ? Result.Fail<T>(new NullOptionError(optionName))
                : Result.Ok(value);
        }

        private Result<string> InitializeFileForReading(string filename)
        {
            if (filename == null)
                return filename.AsResult();
            return FileExtensions.HaveReadAccess(filename)
                .RefineError(error => new ReadInputFileError($"Can't read from input file {filename}", error));
        }

        private Result<string> InitializeFileForWriting(string filename)
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
            return LayouterNames[layouterName](new Point(Width.Value / 2, Height.Value / 2)).AsResult();
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