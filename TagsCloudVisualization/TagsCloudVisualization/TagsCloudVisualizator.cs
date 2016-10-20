using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class VisualizatorConfiguration
    {
        public Func<ICloudLayouter> Layouter { get; set; }
        public Font Font { get; set; }
        public Brush Brush { get; set; }
    }

    public class TagsCloudVisualizator
    {
        public VisualizatorConfiguration Configuration { get; }
        public TagsCloudVisualizator(VisualizatorConfiguration configuration)
        {
            Configuration = configuration;
        }

        /*
         * What parameters configure tags cloud vizualizator?
         *     Layouter, Graphics, Sequence of Words (unique or with replays?), Font
         * 
         * Layouter can be constructor parameter, because Visualizator with another Layouter - another visualizator
         * Font - constructor parameter too ---> VisualizatorConfiguration?
         * Graphics and words - may vary with different queries to out visualizator
         */

        public void CreateTagsCloud(IEnumerable<string> tags, Graphics graphics)
        {
            var layouter = Configuration.Layouter();
            foreach (var tag in tags)
            {
                var boundingBox = graphics.MeasureString(tag, Configuration.Font);
                var finalPosition = layouter.PutNextRectangle(boundingBox.ToSize());
                graphics.DrawString(tag, Configuration.Font, Configuration.Brush, finalPosition.Location);
            }
        }
    }
}