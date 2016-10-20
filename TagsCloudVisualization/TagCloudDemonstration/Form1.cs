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

namespace TagCloudDemonstration
{
    public partial class Form1 : Form
    {
        private VisualizatorConfiguration Configuration { get; set; }
        public Form1()
        {
            InitializeComponent();
            Width = 600;
            Height = 600;
            Configuration = new VisualizatorConfiguration
            {
                Brush = Brushes.DarkSlateBlue,
                Font = new Font(FontFamily.Families[0], 20),
                Layouter = () => new CircularCloudLayouter(new Point(300, 300))
            };
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var visualizator = new TagsCloudVisualizator(Configuration);
            visualizator.CreateTagsCloud(new[] {"hello", "world", "lorem", "ipsum"}, e.Graphics);
        }
    }
}
