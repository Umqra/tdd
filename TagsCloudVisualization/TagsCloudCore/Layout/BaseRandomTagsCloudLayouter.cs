using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;

namespace TagsCloudCore.Layout
{
    public abstract class BaseRandomTagsCloudLayouter : ITagsCloudLayouter
    {
        protected BaseRandomTagsCloudLayouter(Point center)
        {
            Random = new Random(0);
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        private Random Random { get; }
        public Point Center { get; set; }
        public List<Rectangle> Rectangles { get; set; }

        public IEnumerable<Rectangle> GetRectangles()
        {
            return Rectangles;
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

        protected abstract Rectangle ChooseNextPlace(Size rectangleSize);

        protected bool CanPutRectangle(Rectangle rectangle)
        {
            foreach (var other in Rectangles)
            {
                var intersection = other.IntersectWith(rectangle);
                if (intersection != null && !intersection.Value.IsEmpty)
                    return false;
            }
            return true;
        }

        protected IEnumerable<Rectangle> GetRelevantRectanglePositions(Point direction, Size rectangleSize)
        {
            Ray ray = new Ray(Center, Center + direction);
            foreach (var rectangle in Rectangles)
            {
                var extendedRectangle = new Rectangle(rectangle.BottomLeft - rectangleSize / 2,
                    rectangle.Size + rectangleSize);
                foreach (var side in extendedRectangle.Sides)
                {
                    var intersection = side.IntersectWith(ray);
                    if (intersection == null) continue;

                    var current = new Rectangle(intersection.Value - rectangleSize / 2, rectangleSize);
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
}