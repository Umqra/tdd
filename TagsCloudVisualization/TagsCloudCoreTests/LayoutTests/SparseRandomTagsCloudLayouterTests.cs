﻿using System;
using System.Linq;
using FluentAssertions;
using Geometry;
using NUnit.Framework;
using TagsCloudCore.Layout;

namespace TagsCloudCoreTests.LayoutTests
{
    [TestFixture]
    internal class SparseRandomTagsCloudLayouterTests : TagsCloudLayouterTests
    {
        public override ITagsCloudLayouter Layouter { get; set; }
        public override int ScaleFactor => 100;

        [SetUp]
        public void SetUp()
        {
            Layouter = new SparseRandomTagsCloudLayouter(new Point(0, 0));
        }

        public static TestCaseData[] FirstRectangleCases =
        {
            new TestCaseData(new Point(0, 0), new Size(2, 2)),
            new TestCaseData(new Point(10, 10), new Size(2, 2)),
        };
        [TestCaseSource(nameof(FirstRectangleCases))]
        public void FirstRectangle_ShouldContainCenter(Point center, Size rectangleSize)
        {
            Layouter = new SparseRandomTagsCloudLayouter(center);
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
            var rectangles = PutRectanglesOnLayout(rectangleSizes).ToList();

            var secondDirection = rectangles[1].Center - rectangles[0].Center;
            var thirdDirection = rectangles[2].Center - rectangles[0].Center;
            Math.Abs(secondDirection.AngleTo(thirdDirection)).Should().BeGreaterThan(Math.PI / 2);
        }
    }
}