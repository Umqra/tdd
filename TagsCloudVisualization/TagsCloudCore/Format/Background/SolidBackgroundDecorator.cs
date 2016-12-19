using System.Drawing;

namespace TagsCloudCore.Format.Background
{
    public class SolidBackgroundDecorator : IBackgroundDecorator
    {
        private ISolidBackgroundSettings Settings { get; }

        public SolidBackgroundDecorator(ISolidBackgroundSettings settings)
        {
            Settings = settings;
        }

        public void DecorateBackground(Graphics graphics)
        {
            graphics.Clear(Settings.BackgroundColor);
        }
    }
}