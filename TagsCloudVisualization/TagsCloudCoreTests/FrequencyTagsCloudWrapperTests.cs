using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudCore.Format;

namespace TagsCloudCoreTests
{
    [TestFixture]
    class FrequencyTagsCloudWrapperTests
    {
        public const float DefaultFontSize = 40;
        public static readonly FontFamily DefaultFontFamily = FontFamily.GenericSerif;
        public ITagsWrapper FrequencyTagsWrapper { get; set; }

        private FrequencyTagsCloudWrapper InitializeWrapper(IEnumerable<string> tags)
        {
            return new FrequencyTagsCloudWrapper(DefaultFontFamily, DefaultFontSize, tags);
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
