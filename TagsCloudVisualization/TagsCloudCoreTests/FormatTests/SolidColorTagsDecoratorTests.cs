using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudCore.Format;
using Point = Geometry.Point;
using Rectangle = Geometry.Rectangle;

namespace TagsCloudCoreTests.FormatTests
{
    [TestFixture]
    class SolidColorTagsDecoratorTests : TagsDecoratorTests
    {
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Decorator = new SolidColorTagsDecorator(DefaultColor);
        }

        [Test]
        public void ColorForAllTags_ShouldBeSame()
        {
            var firstTag = "first";
            var secondTag = "second";
            var firstCorner = new Point(10, 10);
            var secondCorner = new Point(10, 30);
            Graphics.FromImage(Expected).DrawString(firstTag, DefaultFont, DefaultBrush, (PointF)firstCorner);
            Graphics.FromImage(Expected).DrawString(secondTag, DefaultFont, DefaultBrush, (PointF)secondCorner);

            Decorator.DecorateTag(firstTag, DefaultFont, new Rectangle(firstCorner, GetTagSize(firstTag)),
                Graphics.FromImage(Actual));
            Decorator.DecorateTag(secondTag, DefaultFont, new Rectangle(secondCorner, GetTagSize(secondTag)),
                Graphics.FromImage(Actual));

            BitmapsAreEqual(Actual, Expected).Should().BeTrue();
        }
    }
}
