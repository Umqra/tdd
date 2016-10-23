using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagsCloudCore;

namespace TagsCloudConsole
{
    sealed class CloudDisplayForm : Form
    {
        public TagsCloudVisualizator Visualizator { get; set; }
        public List<string> Tags { get; set; }
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
