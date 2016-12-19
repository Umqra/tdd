using System.Drawing;

namespace TagsCloudCore.Format.Tag.Decorating
{
    public interface IFadedColorTagsDecoratorSettings
    {
        Color BrightColor { get; }
        Color FadeColor { get; }
    }
}