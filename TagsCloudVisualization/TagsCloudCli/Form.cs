using System.Collections.Generic;
using System.Windows.Forms;
using TagsCloudCore.Visualization;

namespace TagsCloudCli
{
    sealed class TagsCloudDisplayForm : Form
    {
        public ITagsCloudVisualizator Visualizator { get; set; }
        public List<string> Tags { get; set; }

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
