using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TagsCloudCore.Format.Tag;
using TagsCloudCore.Format.Tag.Wrapping;
using TagsCloudCore.Tags;

namespace TagsCloudCoreTests.FormatTests
{
    [TestFixture]
    class FrequencyTagsCloudWrapperTests
    {
        public const float DefaultFontSize = 20;
        public static readonly FontFamily DefaultFontFamily = FontFamily.GenericSerif;
        public ITagsWrapper FrequencyTagsWrapper { get; set; }

        private FrequencyTagsCloudWrapper InitializeWrapper(IEnumerable<string> tags)
        {
            var fontProvider = Substitute.For<IFontProvider>();
            fontProvider.GetFont(Arg.Any<float>())
                .ReturnsForAnyArgs(info => new Font(DefaultFontFamily, info.Arg<float>()));
            var tagsCreator = Substitute.For<ITagsCreator>();
            tagsCreator.GetTags().Returns(tags);

            return new FrequencyTagsCloudWrapper(
                fontProvider, 
                new FrequencyWrapperSettings(DefaultFontSize), 
                tagsCreator
            );
        }

        [Test]
        public void SingleTag_MustHaveDefaultFontSize()
        {
            string tag = "single";
            FrequencyTagsWrapper = InitializeWrapper(new[] {tag});

            FrequencyTagsWrapper.GetTagFont(tag).Size.Should().BeApproximately(DefaultFontSize, 1e-9f);
        }

        [Test]
        public void UnknownTag_MustHaveNonZeroSize()
        {
            string tag = "single";
            string unknown = "unknown";
            FrequencyTagsWrapper = InitializeWrapper(new[] {tag});

            FrequencyTagsWrapper.GetTagFont(unknown).Size.Should().BeGreaterThan(0);
        }

        [Test]
        public void UnknownTag_MustHaveNegligibleSize()
        {
            string tag = "single";
            string unknown = "unknown";
            FrequencyTagsWrapper = InitializeWrapper(Enumerable.Repeat(tag, 40));

            FrequencyTagsWrapper.GetTagFont(unknown).Size.Should().BeLessOrEqualTo(5);
        }

        private IEnumerable<string> RepeatEachWordAccordingCounts(IEnumerable<string> tags, IEnumerable<int> counts)
        {
            return
                tags.Zip(counts, Tuple.Create)
                    .Select(tagCounts => Enumerable.Repeat(tagCounts.Item1, tagCounts.Item2))
                    .SelectMany(tag => tag);
        }

        [Test]
        public void TagsSizes_MustDecreaseWithFrequency()
        {
            var tags = new[] {"one", "two", "three"};
            var repeatedTags = RepeatEachWordAccordingCounts(tags, new[] {1, 2, 3});
            FrequencyTagsWrapper = InitializeWrapper(repeatedTags);

            tags.Select(tag => FrequencyTagsWrapper.GetTagFont(tag).Size).Should().BeInAscendingOrder();
        }
    }
}
