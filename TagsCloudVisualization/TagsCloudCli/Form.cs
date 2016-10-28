using System.Collections.Generic;
using System.Windows.Forms;
using TagsCloudCore.Visualization;

namespace TagsCloudCli
{
    // CR: TagsCloud -> CloudDisplay ? It's better to be consistent
    // BTW, cloud is VERY ambiguous nowadays
    sealed class TagsCloudDisplayForm : Form
    {
        public ITagsCloudVisualizator Visualizator { get; set; }
        public List<string> Tags { get; set; }
        // CR: That's a looooong line
        // CR: Empty line needed
        public TagsCloudDisplayForm(ITagsCloudVisualizator visualizator, List<string> tags, int width, int height)
        {
            Visualizator = visualizator;
            Tags = tags;
            Width = width;
            Height = height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Visualizator.CreateTagsCloud(Tags, e.Graphics);
        }
    }
}
