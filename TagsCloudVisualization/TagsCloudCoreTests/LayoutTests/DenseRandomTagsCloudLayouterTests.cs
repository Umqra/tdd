using System.Linq;
using FluentAssertions;
using Geometry;
using NUnit.Framework;
using TagsCloudCore.Layout;

namespace TagsCloudCoreTests.LayoutTests
{
    [TestFixture]
    internal class DenseRandomTagsCloudLayouterTests : TagsCloudLayouterTests
    {
        public override ITagsCloudLayouter Layouter { get; set; }
        public override int ScaleFactor => 100;

        [SetUp]
        public void SetUp()
        {
            Layouter = new DenseRandomTagsCloudLayouter(new Point(0, 0));
        }

        public static TestCaseData[] FirstRectangleCases =
        {
            new TestCaseData(new Point(0, 0), new Size(2, 2)).SetName("Center at zero point"),
            new TestCaseData(new Point(10, 10), new Size(2, 2)).SetName("Center at non-zero point"),
        };
        [TestCaseSource(nameof(FirstRectangleCases))]
        public void FirstRectangle_ShouldContainCenter(Point center, Size rectangleSize)
        {
            Layouter = new DenseRandomTagsCloudLayouter(center);
            var rectangle = Layouter.PutNextRectangle(rectangleSize);

            rectangle.Should().Match<Rectangle>(r => r.Contains(center));
        }

        [TestCase(50)]
        public void Rectangles_ShouldOccupyMostOfFreeSpace(int numberOfRectangles)
        {
            double rectanglesArea = numberOfRectangles;
            var allCorners = PutRectanglesOnLayout(
                       Enumerable.Range(0, numberOfRectangles)
                           .Select(i => new Size(1, 1)))
                   .SelectMany(rectangle => rectangle.Corners);

            var boundingBox = Rectangle.BoundingBoxOf(allCorners);
            rectanglesArea.Should().BeGreaterThan(boundingBox.Value.Area / 2);
        }
    }
}