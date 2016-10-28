using System;
using System.Linq;
using Geometry;

namespace TagsCloudCore.Layout
{
    public class SparseRandomTagsCloudLayouter : DenseRandomTagsCloudLayouter
    {
        public SparseRandomTagsCloudLayouter(Point center) : base(center)
        {
        }
        
        protected override Rectangle ChooseNextPlace(Size rectangleSize)
        {
            var direction = GetSparseDirection();
            return PutNextRectangleAlongDirection(direction, rectangleSize);
        }

        private double GetDirectionCost(Point direction)
        {
            return Rectangles
                .Where(rectangle => !rectangle.Center.Equals(Center))
                .Select(rectangle => Math.Abs((rectangle.Center - Center).AngleTo(direction)))
                .DefaultIfEmpty(0)
                .Min();
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