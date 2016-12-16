using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudCore.Format.Tag
{
    public class FixedFamilyFontProvider : IFontProvider
    {
        public FontFamily FontFamily;
        public FixedFamilyFontProvider(FontFamily fontFamily)
        {
            FontFamily = fontFamily;
        }
        public Font GetFont(float fontEmSize)
        {
            return new Font(FontFamily, fontEmSize);
        }
    }
}
