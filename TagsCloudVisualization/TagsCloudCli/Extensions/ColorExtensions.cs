using System;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudCli.Errors;

namespace TagsCloudCli.Extensions
{
    public static class ColorExtensions
    {
        public static Result<Color> ParseColor(string colorRepresentation)
        {
            if (colorRepresentation[0] == '#')
            {
                if ((colorRepresentation.Length != 7) || !colorRepresentation.Skip(1).All(c => c.IsHex()))
                    return Result.Fail<Color>(new InvalidColorError($"Invalid hex color code {colorRepresentation}"));
                return ColorTranslator.FromHtml(colorRepresentation).AsResult();
            }
            var parsedColor = Color.FromName(colorRepresentation);
            if (!parsedColor.IsKnownColor)
                return Result.Fail<Color>(new InvalidColorError($"Unknown color name: {colorRepresentation}"));
            return parsedColor.AsResult();
        }
    }
}