using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TagsCloudCore;

namespace TagsCloudGenerator
{
    // CR: TagsCloud -> CloudDisplay ? It's better to be consistent
    // BTW, cloud is VERY ambiguous nowadays
    sealed class CloudDisplayForm : Form
    {
        public TagsCloudVisualizator Visualizator { get; set; }
        public List<string> Tags { get; set; }
        // CR: That's a looooong line
        // CR: Empty line needed
        public CloudDisplayForm(TagsCloudVisualizator visualizator, List<string> tags, int width, int height, Color backgroundColor)
        {
            Visualizator = visualizator;
            Tags = tags;
            Width = width;
            Height = height;
            BackColor = backgroundColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Visualizator.CreateTagsCloud(Tags, e.Graphics);
        }
    }
}
