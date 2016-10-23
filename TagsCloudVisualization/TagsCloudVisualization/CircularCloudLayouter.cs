using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private Random Random { get; }
        public Point Center { get; set; }
        public List<Rectangle> Rectangles { get; set; }
        public CircularCloudLayouter(Point center)
        {
            Random = new Random(0);
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle current;
            if (Rectangles.Any())
                current = PutNextRectangleAlongDirection(GetSparseDirection(), rectangleSize);
            else
                current = new Rectangle(Center - rectangleSize / 2, rectangleSize);
            Rectangles.Add(current);
            return current;
        }

        private bool CanPutRectangle(Rectangle rectangle)
        {
            foreach (var otherRectangle in Rectangles)
            {
                var intersection = otherRectangle.IntersectWith(rectangle);
                if (intersection != null && !intersection.IsEmpty)
                    return false;
            }
            return true;
        }

        private IEnumerable<Rectangle> GetPossibleRectanglePositions(Point direction, Size rectangleSize)
        {
            Ray ray = new Ray(Center, Center + direction);
            foreach (var rectangle in Rectangles)
            {
                var extendedRectangle = new Rectangle(rectangle.BottomLeft - rectangleSize / 2, rectangle.Size + rectangleSize);
                foreach (var side in extendedRectangle.Sides)
                {
                    var intersection = side.IntersectWith(ray);
                    if (intersection != null)
                    {
                        var current = new Rectangle(intersection - rectangleSize / 2, rectangleSize);
                        if (CanPutRectangle(current))
                            yield return current;
                    }
                }
            }
        }

        private Rectangle PutNextRectangleAlongDirection(Point direction, Size rectangleSize)
        {
            return
                GetPossibleRectanglePositions(direction, rectangleSize)
                    .OrderBy(rectangle => Center.DistanceTo(rectangle.Center))
                    .First();
        }

        private Point GetRandomDirection()
        {
            return new Point(1, 0).Rotate(Random.NextDouble() * 2 * Math.PI);
        }

        private double GetDirectionCost(Point direction)
        {
            var cost = Rectangles.Select(rectangle => Math.Abs((rectangle.Center - Center).AngleTo(direction))).Min();
            return cost;
        }

        private Point GetSparseDirection()
        {
            var candidates = Enumerable.Range(0, 10)
                .Select(i => GetRandomDirection());
            return candidates
                .OrderByDescending(GetDirectionCost)
                .First();
        }
    }
}
