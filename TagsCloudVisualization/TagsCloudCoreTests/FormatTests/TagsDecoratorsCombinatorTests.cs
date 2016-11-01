using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Geometry;
using NSubstitute;
using NUnit.Framework;
using TagsCloudCore.Format;

namespace TagsCloudCoreTests
{
    [TestFixture]
    class TagsDecoratorsCombinatorTests
    {
        [Test]
        public void TwoDecorators_ShouldBeCalled()
        {
            var left = Substitute.For<ITagsDecorator>();
            var right = Substitute.For<ITagsDecorator>();
            var combination = left.With(right);
            combination.DecorateTag(null, null, default(Geometry.Rectangle), null);

            left.Received().DecorateTag(Arg.Any<string>(), Arg.Any<Font>(), Arg.Any<Geometry.Rectangle>(), Arg.Any<Graphics>());
            right.Received().DecorateTag(Arg.Any<string>(), Arg.Any<Font>(), Arg.Any<Geometry.Rectangle>(), Arg.Any<Graphics>());
        }

        [Test]
        public void AnyDecorator_ShouldBeCalled()
        {
            var decorators = Enumerable.Range(0, 5).Select(i => Substitute.For<ITagsDecorator>()).ToArray();
            var combination = decorators[0]
                .With(decorators[1].With(decorators[2]))
                .With(decorators[3]).With(decorators[4]);

            combination.DecorateTag(null, null, default(Geometry.Rectangle), null);

            foreach (var d in decorators)
                d.Received().DecorateTag(null, null, default(Geometry.Rectangle), null);
        }
    }
}

