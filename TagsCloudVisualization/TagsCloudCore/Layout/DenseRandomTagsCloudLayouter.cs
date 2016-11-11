using System.Linq;
using Geometry;

namespace TagsCloudCore.Layout
{
    public class DenseRandomTagsCloudLayouter : BaseRandomTagsCloudLayouter
    {
        public DenseRandomTagsCloudLayouter(Point center) : base(center)
        {
        }

        private int NumberOfRandomDirections = 10;
        protected override Rectangle ChooseNextPlace(Size rectangleSize)
        {
            return Enumerable.Range(0, NumberOfRandomDirections)
                .Select(i => PutNextRectangleAlongDirection(GetRandomDirection(), rectangleSize))
                .OrderBy(rectangle => Center.DistanceTo(rectangle.Center))
                .First();
        }
    }
}
