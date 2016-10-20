using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;

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
            new TestCaseData(new Size(2, 2), new Size(2, 2)),
            new TestCaseData(new Size(1, 2), new Size(2, 1)),
            new TestCaseData(new Size(2, 5), new Size(3, 4))
        };
        [TestCaseSource(nameof(TwoRectangleParameters))]
        public void TwoRectangleCanNotOverlap(Size firstRectangleSize, Size secondRectangleSize)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var firstRectangle = layouter.PutNextRectangle(firstRectangleSize);
            var secondRectangle = layouter.PutNextRectangle(secondRectangleSize);
            firstRectangle.Should().NotMatch<Rectangle>(rectangle => rectangle.IntersectsWith(secondRectangle));
        }

        [TestCase(5)]
        [TestCase(10)]
        public void ManyRectanglesMustAppearsInAnyQuater(int amount)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            Enumerable.Range(0, amount)
                .Select(i => layouter.PutNextRectangle(new Size(1, 1)))
                .Select(rect => rect.GetCenter().GetQuarter())
                .Distinct()
                .Should().HaveCount(5);
        }
    }
}
