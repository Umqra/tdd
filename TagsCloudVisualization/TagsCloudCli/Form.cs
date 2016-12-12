using System.Collections.Generic;
using System.Windows.Forms;
using TagsCloudCore.Visualization;

namespace TagsCloudCli
{
    sealed class TagsCloudDisplayForm : Form
    {
        public ITagsCloudVisualizator Visualizator { get; set; }

        public TagsCloudDisplayForm(ITagsCloudVisualizator visualizator, int width, int height)
        {
            Visualizator = visualizator;
            Width = width;
            Height = height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Visualizator.CreateTagsCloud(e.Graphics);
        }
    }
}
