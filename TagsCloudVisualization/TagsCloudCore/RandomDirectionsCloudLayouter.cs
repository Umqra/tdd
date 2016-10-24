using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace TagsCloudCore
{
    public class RandomDirectionsCloudLayouter : ICloudLayouter
    {
        private Random Random { get; }
        public Point Center { get; set; }
        public List<Rectangle> Rectangles { get; set; }

        public RandomDirectionsCloudLayouter(Point center)
        {
            Random = new Random(0);
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle current;
            if (Rectangles.Any())
                current = ChooseNextPlace(rectangleSize);
            else
                current = new Rectangle(Center - rectangleSize / 2, rectangleSize);
            Rectangles.Add(current);
            return current;
        }

        private int NumberOfRandomDirections = 10;
        protected virtual Rectangle ChooseNextPlace(Size rectangleSize)
        {
            return Enumerable.Range(0, NumberOfRandomDirections)
                .Select(i => PutNextRectangleAlongDirection(GetRandomDirection(), rectangleSize))
                .OrderBy(rectangle => Center.DistanceTo(rectangle.Center))
                .First();
        }

        protected bool CanPutRectangle(Rectangle rectangle)
        {
            foreach (var other in Rectangles)
            {
                var intersection = other.IntersectWith(rectangle);
                if (intersection != null && !intersection.IsEmpty)
                    return false;
            }
            return true;
        }

        protected IEnumerable<Rectangle> GetRelevantRectanglePositions(Point direction, Size rectangleSize)
        {
            Ray ray = new Ray(Center, Center + direction);
            foreach (var rectangle in Rectangles)
            {
                var extendedRectangle = new Rectangle(rectangle.BottomLeft - rectangleSize / 2, rectangle.Size + rectangleSize);
                foreach (var side in extendedRectangle.Sides)
                {
                    var intersection = side.IntersectWith(ray);
                    if (intersection == null) continue;

                    var current = new Rectangle(intersection - rectangleSize / 2, rectangleSize);
                    if (CanPutRectangle(current))
                        yield return current;
                }
            }
        }

        protected Rectangle PutNextRectangleAlongDirection(Point direction, Size rectangleSize)
        {
            return GetRelevantRectanglePositions(direction, rectangleSize)
                .OrderBy(rectangle => Center.DistanceTo(rectangle.Center))
                .First();
        }

        protected Point GetRandomDirection()
        {
            return new Point(1, 0).Rotate(Random.NextDouble() * 2 * Math.PI);
        }
    }

    public class RandomSparseCloudLayouter : RandomDirectionsCloudLayouter
    {
        public RandomSparseCloudLayouter(Point center) : base(center)
        {
        }

        private int NumberOfRandomDirections = 10;
        protected override Rectangle ChooseNextPlace(Size rectangleSize)
        {
            var direction = Enumerable.Range(0, NumberOfRandomDirections)
                .Select(i => GetRandomDirection())
                .OrderBy(GetDirectionCost)
                .First();
            return PutNextRectangleAlongDirection(direction, rectangleSize);
        }

        private double GetDirectionCost(Point direction)
        {
            return Rectangles.Select(rectangle => Math.Abs((rectangle.Center - Center).AngleTo(direction))).Min();
        }

        private Point GetSparseDirection(int numberOfRandomDirections = 10)
        {
            var candidates = Enumerable.Range(0, numberOfRandomDirections)
                .Select(i => GetRandomDirection());
            return candidates
                .OrderByDescending(GetDirectionCost)
                .First();
        }
    }
}
