using System;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace TagsCloudCoreTests
{
    public static class FluentAssertionsExtensions
    {
        public static AndConstraint<ObjectAssertions> NotMatch<T>(this ObjectAssertions assertion, Predicate<T> predicate, string because = "", params object[] becauseArgs)
        {
            return assertion.Match<T>(obj => !predicate(obj), because, becauseArgs);
        }
    }
}