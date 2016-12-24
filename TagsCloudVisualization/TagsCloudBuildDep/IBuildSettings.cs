using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudCore.Format.Background;
using TagsCloudCore.Format.Tag;
using TagsCloudCore.Format.Tag.Decorating;
using TagsCloudCore.Format.Tag.Wrapping;
using TagsCloudCore.Tags;
using TagsCloudCore.Tags.Preparers;

namespace TagsCloudBuildDep
{
    public interface IBuildSettings :
        IFadedColorTagsDecoratorSettings, 
        IShadedBackgroundTagsDecoratorSettings,
        ISolidColorTagsDecoratorSettings,
        IFirstTagsTakerSettings,
        IFrequencyTagsCloudWrapperSettings,
        ISolidBackgroundSettings,
        ITagsCreatorSettings,
        IFixedFamilyFontProviderSettings
    {
    }
}
