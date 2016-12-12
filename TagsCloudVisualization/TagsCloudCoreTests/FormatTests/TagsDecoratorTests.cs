using System.Drawing;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudCore.Format.Tag.Decorating;
using TagsCloudCore.Format.Tag.Wrapping;
using Size = Geometry.Size;

namespace TagsCloudCoreTests.FormatTests
{
    public abstract class TagsDecoratorTests
    {
        private const float DefaultFontSize = 20;
        private const int DefaultHeight = 200;
        private const int DefaultWidth = 200;
        protected readonly Color DefaultColor = Color.Blue;
        protected Bitmap Actual { get; set; }
        protected Bitmap Expected { get; set; }
        protected ITagsDecorator Decorator { get; set; }

        protected Font DefaultFont => new Font(FontFamily.GenericSerif, DefaultFontSize);
        protected Brush DefaultBrush => new SolidBrush(DefaultColor);


        [SetUp]
        public virtual void SetUp()
        {
            Actual = new Bitmap(DefaultWidth, DefaultHeight);
            Graphics.FromImage(Actual).Clear(Color.White);

            Expected = new Bitmap(DefaultWidth, DefaultHeight);
            Graphics.FromImage(Expected).Clear(Color.White);
        }

        [TearDown]
        public void TearDown()
        {
            var testContext = TestContext.CurrentContext;
            if (testContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            var actualDestination = Path.Combine(testContext.TestDirectory, testContext.Test.FullName + "_actual.bmp");
            var expectedDestination = Path.Combine(testContext.TestDirectory,
                testContext.Test.FullName + "_expected.bmp");
            Actual.Save(actualDestination);
            Expected.Save(expectedDestination);
            TestContext.Out.Write("Generated images written in {0} and {1}", actualDestination, expectedDestination);
        }

        protected Size GetTagSize(string tag)
        {
            return new FrequencyTagsCloudWrapper(
                size => new Font(FontFamily.GenericSerif, size), DefaultFontSize, new[] {tag}
            ).MeasureTag(tag, Graphics.FromImage(Actual));
        }

        protected bool BitmapsAreEqual(Bitmap a, Bitmap b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                return false;
            for (int i = 0; i < a.Width; i++)
                for (int s = 0; s < a.Height; s++)
                {
                    if (a.GetPixel(i, s) != b.GetPixel(i, s))
                        return false;
                }
            return true;
        }
    }
}