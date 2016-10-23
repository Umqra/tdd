using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagsCloudVisualization;
using Geometry;

namespace TagCloudDemonstration
{
    public sealed partial class Form1 : Form
    {
        private List<string> Tags { get; set; }
        private VisualizatorConfiguration Configuration { get; set; }
        public Form1()
        {
            InitializeComponent();
            Width = 800;
            Height = 800;

            BackColor = Color.Black;
            Tags = new TagsExtractor().ExtractFromFile("text.txt").ToList();
            Configuration = new VisualizatorConfiguration
            {
                Layouter = () => new CircularCloudLayouter(new Geometry.Point(400, 400)),
                Formatter = () => new FrequencyCloudFormatter(FontFamily.GenericSerif, 50, Brushes.DarkOrchid, Tags)
            }; 
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var visualizator = new TagsCloudVisualizator(Configuration);
            visualizator.CreateTagsCloud(Tags.Distinct(), e.Graphics);
        }
    }
}
