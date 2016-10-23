using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;
using Geometry;

// ReSharper disable InconsistentNaming

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        public static TestCaseData[] FirstRectangleParameters = 
        {
            new TestCaseData(new Point(0, 0), new Size(2, 2)),
            new TestCaseData(new Point(10, 10), new Size(2, 2)),
        };
        [TestCaseSource(nameof(FirstRectangleParameters))]
        public void FirstRectangleMustContainCenter(Point center, Size rectangleSize)
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangle = layouter.PutNextRectangle(rectangleSize);

            rectangle.Should().Match<Rectangle>(r => r.Contains(center));
        }

        public static TestCaseData[] TwoRectangleParameters =
        { 
            new TestCaseData((object)new [] {new Size(2, 2), new Size(2, 2)}),
            new TestCaseData((object)new [] {new Size(1, 2), new Size(2, 1)}),
            new TestCaseData((object)new [] {new Size(2, 5), new Size(3, 4)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(3, 1), new Size(3, 1)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(1, 1), new Size(4, 2), new Size(3, 3)})
        };
        [TestCaseSource(nameof(TwoRectangleParameters))]
        public void RectanglesCanNotOverlap(Size[] rectangleSizes)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            foreach (var size in rectangleSizes)
                rectangles.Add(layouter.PutNextRectangle(size));
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
        public void EachRectangleMustTouchAnother(Size[] rectangleSizes)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            foreach (var size in rectangleSizes)
                rectangles.Add(layouter.PutNextRectangle(size));
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
        public void ManyRectanglesMustAppearsInAnyQuater(int amount)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            Enumerable.Range(0, amount)
                .Select(i => layouter.PutNextRectangle(new Size(1, 1)))
                .Select(rect => rect.Center.Quater)
                .Distinct()
                .Should().HaveCount(5);
        }

        public static TestCaseData[] OppositeDirectionCases =
        {
            new TestCaseData((object)new [] {new Size(2, 2), new Size(2, 2), new Size(2, 2)}),
            new TestCaseData((object)new [] {new Size(3, 1), new Size(3, 1), new Size(3, 1)}),
        };
        [TestCaseSource(nameof(OppositeDirectionCases))]
        public void SecondAndThirdRectanglesInOppositeDirections(Size[] rectangleSizes)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            foreach (var size in rectangleSizes)
                rectangles.Add(layouter.PutNextRectangle(size));
            Point secondDirection = rectangles[1].Center - rectangles[0].Center;
            Point thirdDirection = rectangles[2].Center - rectangles[0].Center;
            Math.Abs(secondDirection.AngleTo(thirdDirection)).Should().BeGreaterThan(Math.PI / 2);
        }
    }
}
