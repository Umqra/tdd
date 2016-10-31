using System;
using System.Collections.Generic;
using System.Text;
using Fclp;
using Fclp.Internals;

namespace TagsCloudCli
{
    public class CustomCommandLineOptionFormatter : ICommandLineOptionFormatter
    {
        private const int TextWidth = 80;
        private const string LinePadding = "    ";

        public string Format(IEnumerable<ICommandLineOption> options)
        {
            var formattedOptions = new StringBuilder();
            foreach (var option in options)
                formattedOptions.AppendLine(CreateOptionLine(option));
            return formattedOptions.ToString().TrimEnd();
        }

        private string CreateOptionLine(ICommandLineOption option)
        {
            var optionLine = LinePadding;
            optionLine += string.Join(", ", GetNamesRepresentation(option));
            if (optionLine.Length * 2 > TextWidth)
                optionLine += "\n" + LinePadding + LinePadding + WrapString(2 * LinePadding.Length, option.Description);
            else
            {
                var separator = new string(' ', Math.Max(0, 20 - optionLine.Length)) + new string(' ', 5);
                optionLine += separator;
                optionLine += WrapString(optionLine.Length, option.Description);
            }
            if (optionLine.Length > TextWidth)
                optionLine += "\n";

            return optionLine;
        }

        private string WrapString(int startOffset, string text)
        {
            var wrapped = "";
            var offset = startOffset;
            foreach (var token in text.Split())
            {
                if (offset > TextWidth)
                {
                    offset = 2 * LinePadding.Length;
                    wrapped += "\n" + LinePadding + LinePadding;
                }
                wrapped += token + " ";
            }
            return wrapped;
        }

        private IEnumerable<string> GetNamesRepresentation(ICommandLineOption option)
        {
            if (option.HasShortName)
                yield return $"-{option.ShortName}";
            if (option.HasLongName)
                yield return $"--{option.LongName}";
        }
    }
}