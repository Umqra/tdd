using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudCore.Format;
using Point = Geometry.Point;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCoreTests
{
    class FadedColorTagsDecoratorTests : TagsDecoratorTests
    {
        private readonly Color fadedColor = Color.LightGoldenrodYellow;
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Decorator = new FadedColorTagsDecorator(defaultColor, fadedColor);
        }

        private IEnumerable<Color> EnumerateColorsFromLines(Bitmap image, IEnumerable<int> lineIds)
        {
            foreach (int y in lineIds)
                for (int x = 0; x < image.Width; x++)
                    yield return image.GetPixel(x, y);
        }

        private int Epsilon = 5;

        private bool ColorsAreFading(Color bright, Color faded)
        {
            return Math.Abs(bright.R - fadedColor.R) + Epsilon >= Math.Abs(faded.R - fadedColor.R) &&
                   Math.Abs(bright.G - fadedColor.G) + Epsilon >= Math.Abs(faded.G - fadedColor.G) &&
                   Math.Abs(bright.B - fadedColor.B) + Epsilon >= Math.Abs(faded.B - fadedColor.B);
        }

        private bool ColorsAreFading(Tuple<Color, Color, Color> colors)
        {
            return ColorsAreFading(colors.Item1, colors.Item2) && ColorsAreFading(colors.Item2, colors.Item3);
        }

        [Test]
        public void FontColor_ShouldBeFading_FarAwayFromCenter()
        {
            var tags = new[] {"test", "test", "test"};
            var corners = new[] {new Point(10, 100), new Point(10, 140), new Point(10, 180)};

            var graphics = Graphics.FromImage(Actual);
            for (int i = 0; i < tags.Length; i++)
                Decorator.DecorateTag(tags[i], DefaultFont, new Rectangle(corners[i], GetTagSize(tags[i])), graphics);

            var gropedTags = EnumerateColorsFromLines(Actual, Enumerable.Range((int)corners[0].Y, 10))
                .Zip(EnumerateColorsFromLines(Actual, Enumerable.Range((int)corners[1].Y, 10)), Tuple.Create)
                .Zip(EnumerateColorsFromLines(Actual, Enumerable.Range((int)corners[2].Y, 10)),
                    (tuple, a) => Tuple.Create(tuple.Item1, tuple.Item2, a));

            foreach (var group in gropedTags)
                group.Should().Match<Tuple<Color, Color, Color>>(tuple => ColorsAreFading(tuple));
        }
    }
}
