using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudCore;
using TagsCloudCore.Layout;
using Rectangle = Geometry.Rectangle;
using Size = Geometry.Size;

namespace TagsCloudCoreTests.LayoutTests
{
    internal abstract class TagsCloudLayouterTests
    {
        public abstract ITagsCloudLayouter Layouter { get; set; }
        public abstract int ScaleFactor { get; }

        public IEnumerable<Rectangle> PutRectanglesOnLayout(IEnumerable<Size> sizes)
        {
            foreach (var size in sizes)
                yield return Layouter.PutNextRectangle(size);
        }

        public static TestCaseData[] RectanglesSizeCases =
        {
            new TestCaseData((object)new [] {new Size(2, 2), new Size(2, 2)}).SetName("Two equal squares"),
            new TestCaseData((object)new [] {new Size(1, 2), new Size(2, 1)}).SetName("Wide and long rectangles"),
            new TestCaseData((object)new [] {new Size(2, 5), new Size(3, 4)}).SetName("Two random rectangles"),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(3, 1), new Size(3, 1)}).SetName("Equal rectangles"),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(1, 1), new Size(4, 2), new Size(3, 3)}).SetName("Many rectangles")
        };
        [TestCaseSource(nameof(RectanglesSizeCases))]
        public void Rectangles_ShouldNotOverlap(Size[] rectangleSizes)
        {
            var rectangles = PutRectanglesOnLayout(rectangleSizes).ToList();

            for (int i = 0; i < rectangles.Count; i++)
                for (int s = i + 1; s < rectangles.Count; s++)
                {
                    var intersection = rectangles[i].IntersectWith(rectangles[s]);
                    intersection.Should().Match<Rectangle?>(rectangle => rectangle == null || rectangle.Value.IsEmpty);
                }
        }
        
        [TestCaseSource(nameof(RectanglesSizeCases))]
        public void ShouldTouchOneAnother(Size[] rectangleSizes)
        {
            var rectangles = PutRectanglesOnLayout(rectangleSizes).ToList();

            for (int i = 0; i < rectangles.Count; i++)
            {
                rectangles
                    .Where((other, s) => i != s && rectangles[i].Touches(other))
                    .Any()
                    .Should()
                    .BeTrue();
            }
        }
        
        [TestCase(10)]
        public void Rectangles_ShouldAppearInAnyQuater(int numberOfRectangles)
        {
            var rectangles = PutRectanglesOnLayout(
                Enumerable.Range(0, numberOfRectangles)
                    .Select(i => new Size(1, 1)));

            rectangles
                .Select(rect => rect.Center.Quater)
                .Distinct()
                .Should().HaveCount(5);
        }

        [TestCase(10)]
        [TestCase(20)]
        public void BoundingBox_ShouldBeApproximatelySquare(int numberOfRectangles)
        {
            var allCorners = PutRectanglesOnLayout(
                    Enumerable.Range(0, numberOfRectangles)
                        .Select(i => new Size(1, 1)))
                .SelectMany(rectangle => rectangle.Corners);

            var boundingBox = Rectangle.BoundingBoxOf(allCorners).Value;
            boundingBox.Size.Width.Should()
                .BeGreaterThan(boundingBox.Size.Height / 2)
                .And
                .BeLessThan(boundingBox.Size.Height * 2);
        }

        [TearDown]
        public void TearDown()
        {
            var testContext = TestContext.CurrentContext;
            if (testContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            var image = Layouter.GetRectangles().CreateImage(ScaleFactor, new SolidBrush(Color.FromArgb(100, 100, 100, 255)));
            var imageDestination = Path.Combine(testContext.TestDirectory, testContext.Test.FullName + ".bmp");
            image.Save(imageDestination);
            TestContext.Out.Write("Generated layout written in {0}", imageDestination);
        }
    }
}
