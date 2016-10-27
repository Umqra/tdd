using System;
using System.Collections.Generic;
using System.Drawing;
// CR: Don't forget to remove unused references
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudCore;
using Point = Geometry.Point;
using Rectangle = Geometry.Rectangle;
using Size = Geometry.Size;

// CR: Don't use this unless you ABSOLUTELY MUST
//! ReSharper disable InconsistentNaming

namespace TagsCloudCoreTests
{
    // CR: Why test classes need to be public?
    public abstract class CloudLayouterTests
    {
        public abstract ICloudLayouter Layouter { get; set; }
        public abstract int ScaleFactor { get; }

        public static TestCaseData[] TwoRectangleCases =
        { 
            new TestCaseData((object)new [] {new Size(2, 2), new Size(2, 2)}),
            new TestCaseData((object)new [] {new Size(1, 2), new Size(2, 1)}),
            new TestCaseData((object)new [] {new Size(2, 5), new Size(3, 4)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(3, 1), new Size(3, 1)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(1, 1), new Size(4, 2), new Size(3, 3)})
        };
        [TestCaseSource(nameof(TwoRectangleCases))]
        public void Rectangles_ShouldNotOverlap(Size[] rectangleSizes)
        {
            var rectangles = new List<Rectangle>();
            foreach (var size in rectangleSizes)
                rectangles.Add(Layouter.PutNextRectangle(size));
            for (int i = 0; i < rectangles.Count; i++)
                for (int s = i + 1; s < rectangles.Count; s++)
                {
                    var intersection = rectangles[i].IntersectWith(rectangles[s]);
                    intersection.Should().Match<Rectangle>(rectangle => rectangle == null || rectangle.IsEmpty);
                }
        }

        public static TestCaseData[] TouchCases =
        {
            new TestCaseData((object)new [] {new Size(2, 2), new Size(2, 2)}),
            new TestCaseData((object)new [] {new Size(1, 2), new Size(2, 1)}),
            new TestCaseData((object)new [] {new Size(2, 5), new Size(3, 4)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(3, 1), new Size(3, 1)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(1, 1), new Size(4, 2), new Size(3, 3)})
        };
        [TestCaseSource(nameof(TouchCases))]
        // CR: Mb 'ShouldTouchOneAnother'?
        public void EachRectangle_ShouldTouchesAnother(Size[] rectangleSizes)
        {
            var rectangles = new List<Rectangle>();
            foreach (var size in rectangleSizes)
                rectangles.Add(Layouter.PutNextRectangle(size));
            // CR: I think new line between Arrange, Act and Assert seems nice
            // Not a strict requirement, but consider using it
            // CR: Maybe make it nicer with some LINQ?
            for (int i = 0; i < rectangles.Count; i++)
            {
                bool touchAny = false;
                for (int s = 0; s < rectangles.Count; s++)
                {
                    if (s == i)
                        continue;
                    touchAny |= rectangles[i].Touches(rectangles[s]);
                }
                touchAny.Should().BeTrue();
            }
        }
        
        [TestCase(10)]
        public void Rectangles_ShouldAppearInAnyQuater(int numberOfRectangles)
        {
            // CR: Act and Assert don't look very distinct here
            Enumerable.Range(0, numberOfRectangles)
                .Select(i => Layouter.PutNextRectangle(new Size(1, 1)))
                .Select(rect => rect.Center.Quater)
                .Distinct()
                .Should().HaveCount(5);
        }

        // CR: I thought we need a circle, not square :D
        [TestCase(10)]
        [TestCase(20)]
        public void BoundingBox_ShouldBeApproximatelySquare(int numberOfRectangles)
        {
            IEnumerable<Point> allCorners = Enumerable.Empty<Point>();
            for (int i = 0; i < numberOfRectangles; i++)
                allCorners = allCorners.Concat(Layouter.PutNextRectangle(new Size(1, 1)).Corners);
            Rectangle boundingBox = Rectangle.BoundingBoxOf(allCorners);
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
            var image = Layouter.Rectangles.CreateImage(ScaleFactor, new SolidBrush(Color.FromArgb(100, 100, 100, 255)));
            var imageDestination = Path.Combine(testContext.TestDirectory, testContext.Test.FullName + ".bmp");
            image.Save(imageDestination);
            TestContext.Out.Write("Generated layout written in {0}", imageDestination);
        }
    }

    // CR: 1 class = 1 file
    [TestFixture]
    public class RandomDirectionsCloudLayouterTests : CloudLayouterTests
    {
        public override ICloudLayouter Layouter { get; set; }
        public override int ScaleFactor => 100;

        [SetUp]
        public void SetUp()
        {
            Layouter = new RandomDirectionsCloudLayouter(new Point(0, 0));
        }

        public static TestCaseData[] FirstRectangleCases =
        {
            new TestCaseData(new Point(0, 0), new Size(2, 2)),
            new TestCaseData(new Point(10, 10), new Size(2, 2)),
        };
        [TestCaseSource(nameof(FirstRectangleCases))]
        public void FirstRectangle_ShouldContainCenter(Point center, Size rectangleSize)
        {
            Layouter = new RandomDirectionsCloudLayouter(center);
            var rectangle = Layouter.PutNextRectangle(rectangleSize);
            rectangle.Should().Match<Rectangle>(r => r.Contains(center));
        }

        [TestCase(50)]
        public void Rectangles_ShouldOccupyMostOfFreeSpace(int numberOfRectangles)
        {
            // CR: Are you sure you need to specify variables types? Especially in tests
            IEnumerable<Point> allCorners = Enumerable.Empty<Point>();
            double rectanglesArea = numberOfRectangles;
            for (int i = 0; i < numberOfRectangles; i++)
                allCorners = allCorners.Concat(Layouter.PutNextRectangle(new Size(1, 1)).Corners);
            Rectangle boundingBox = Rectangle.BoundingBoxOf(allCorners);

            rectanglesArea.Should().BeGreaterThan(boundingBox.Area / 2);
        }
    }

    [TestFixture]
    public class RandomSparseCloudLayouterTests : CloudLayouterTests
    {
        public override ICloudLayouter Layouter { get; set; }
        public override int ScaleFactor => 100;

        [SetUp]
        public void SetUp()
        {
            Layouter = new RandomSparseCloudLayouter(new Point(0, 0));
        }

        public static TestCaseData[] FirstRectangleCases =
        {
            new TestCaseData(new Point(0, 0), new Size(2, 2)),
            new TestCaseData(new Point(10, 10), new Size(2, 2)),
        };
        [TestCaseSource(nameof(FirstRectangleCases))]
        public void FirstRectangle_ShouldContainCenter(Point center, Size rectangleSize)
        {
            Layouter = new RandomSparseCloudLayouter(center);
            var rectangle = Layouter.PutNextRectangle(rectangleSize);
            rectangle.Should().Match<Rectangle>(r => r.Contains(center));
        }

        public static TestCaseData[] OppositeDirectionCases =
        {
            new TestCaseData((object)new [] {new Size(2, 2), new Size(2, 2), new Size(2, 2)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(1, 3), new Size(2, 4)}),
        };
        [TestCaseSource(nameof(OppositeDirectionCases))]
        public void SecondAndThirdRectangles_LocatedInOppositeDirections(Size[] rectangleSizes)
        {
            var rectangles = new List<Rectangle>();
            foreach (var size in rectangleSizes)
                rectangles.Add(Layouter.PutNextRectangle(size));
            Point secondDirection = rectangles[1].Center - rectangles[0].Center;
            Point thirdDirection = rectangles[2].Center - rectangles[0].Center;
            Math.Abs(secondDirection.AngleTo(thirdDirection)).Should().BeGreaterThan(Math.PI / 2);
        }
    }
}
