using System.Linq;
using FluentAssertions;
using Geometry;
using NUnit.Framework;
using TagsCloudCore.Layout;

namespace TagsCloudCoreTests
{
    [TestFixture]
    internal class RandomDenseCloudLayouterTests : CloudLayouterTests
    {
        public override ITagsCloudLayouter Layouter { get; set; }
        public override int ScaleFactor => 100;

        [SetUp]
        public void SetUp()
        {
            Layouter = new RandomDenseTagsCloudLayouter(new Point(0, 0));
        }

        public static TestCaseData[] FirstRectangleCases =
        {
            new TestCaseData(new Point(0, 0), new Size(2, 2)).SetName("Center at zero point"),
            new TestCaseData(new Point(10, 10), new Size(2, 2)).SetName("Center at non-zero point"),
        };
        [TestCaseSource(nameof(FirstRectangleCases))]
        public void FirstRectangle_ShouldContainCenter(Point center, Size rectangleSize)
        {
            Layouter = new RandomDenseTagsCloudLayouter(center);
            var rectangle = Layouter.PutNextRectangle(rectangleSize);

            rectangle.Should().Match<Rectangle>(r => r.Contains(center));
        }

        [TestCase(50)]
        public void Rectangles_ShouldOccupyMostOfFreeSpace(int numberOfRectangles)
        {
            // CR: Are you sure you need to specify variables types? Especially in tests
            var allCorners = Enumerable.Empty<Point>();
            double rectanglesArea = numberOfRectangles;
            for (int i = 0; i < numberOfRectangles; i++)
                allCorners = allCorners.Concat(Layouter.PutNextRectangle(new Size(1, 1)).Corners);

            var boundingBox = Rectangle.BoundingBoxOf(allCorners);
            rectanglesArea.Should().BeGreaterThan(boundingBox.Area / 2);
        }
    }
}