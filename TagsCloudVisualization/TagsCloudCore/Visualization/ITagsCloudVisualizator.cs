using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudCore.Visualization
{
    public interface ITagsCloudVisualizator
    {
        void CreateTagsCloud(Graphics graphics);
    }
}