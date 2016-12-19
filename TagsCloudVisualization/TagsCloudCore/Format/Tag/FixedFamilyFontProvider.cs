using System.Drawing;

namespace TagsCloudCore.Format.Tag
{
    public class FixedFamilyFontProvider : IFontProvider
    {
        private IFixedFamilyFontProviderSettings Settings { get; }

        public FixedFamilyFontProvider(IFixedFamilyFontProviderSettings settings)
        {
            Settings = settings;
        }

        public Font GetFont(float fontEmSize)
        {
            return new Font(Settings.FontFamily, fontEmSize);
        }
    }
}