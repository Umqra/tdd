using System;
using System.Drawing;
using System.IO;
using TagsCloudCli.Extensions;
using TagsCloudCore.Layout;
using Point = Geometry.Point;

namespace TagsCloudCli
{
    public partial class CliOptions
    {
        private void InitializeInputFile(string filename)
        {
            if (!FileExtensions.HaveReadAccess(filename))
                throw new IOException($"Can't read from input file {filename}");
        }

        private void InitializeOutputFile(string filename)
        {
            if (filename != null && !FileExtensions.HaveWriteAccess(filename))
                throw new IOException($"Can't write to output file {filename}");
        }

        private Color InitializeColor(string colorRepresentation)
        {
            return ColorExtensions.ParseColor(colorRepresentation);
        }

        private ITagsCloudLayouter InitializeLayouter(string layouterName)
        {
            if (!LayouterNames.ContainsKey(layouterName))
                throw new FormatException(
                    $"Unknown layouter name {layouterName}. You can use one from the list: {String.Join(", ", LayouterNames.Keys)}");
            return LayouterNames[layouterName](new Point(Width / 2, Height / 2));
        }
    }
}
