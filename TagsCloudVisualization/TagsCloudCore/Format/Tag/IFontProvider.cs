using System.Drawing;

namespace TagsCloudCore.Format.Tag
{
    public interface IFontProvider
    {
        Font GetFont(float fontEmSize);
    }
}