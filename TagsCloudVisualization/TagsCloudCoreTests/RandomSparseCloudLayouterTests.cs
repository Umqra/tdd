using System;
using System.Collections.Generic;
using FluentAssertions;
using Geometry;
using NUnit.Framework;
using TagsCloudCore.Layout;

namespace TagsCloudCoreTests
{
    [TestFixture]
    internal class RandomSparseCloudLayouterTests : CloudLayouterTests
    {
        public override ITagsCloudLayouter Layouter { get; set; }
        public override int ScaleFactor => 100;

        [SetUp]
        public void SetUp()
        {
            Layouter = new RandomSparseTagsCloudLayouter(new Point(0, 0));
        }

        public static TestCaseData[] FirstRectangleCases =
        {
            new TestCaseData(new Point(0, 0), new Size(2, 2)),
            new TestCaseData(new Point(10, 10), new Size(2, 2)),
        };
        [TestCaseSource(nameof(FirstRectangleCases))]
        public void FirstRectangle_ShouldContainCenter(Point center, Size rectangleSize)
        {
            Layouter = new RandomSparseTagsCloudLayouter(center);
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

            var secondDirection = rectangles[1].Center - rectangles[0].Center;
            var thirdDirection = rectangles[2].Center - rectangles[0].Center;
            Math.Abs(secondDirection.AngleTo(thirdDirection)).Should().BeGreaterThan(Math.PI / 2);
        }
    }
}