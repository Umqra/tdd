using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudCli.Extensions
{
    public static class ColorExtensions
    {
        public static Color ParseColor(string colorRepresentation)
        {
            if (colorRepresentation[0] == '#')
            {
                if ((colorRepresentation.Length != 7) || !colorRepresentation.Skip(1).All(c => c.IsHex()))
                    throw new FormatException($"Invalid hex color code {colorRepresentation}");
                return ColorTranslator.FromHtml(colorRepresentation);
            }
            var parsedColor = Color.FromName(colorRepresentation);
            if (!parsedColor.IsKnownColor)
                throw new FormatException($"Unknown color name: {colorRepresentation}");
            return parsedColor;
        }
    }
}