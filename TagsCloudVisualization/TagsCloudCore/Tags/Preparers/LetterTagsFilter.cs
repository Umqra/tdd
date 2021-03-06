﻿using System.Collections.Generic;
using System.Linq;

namespace TagsCloudCore.Tags.Preparers
{
    public class LetterTagsFilter : ITagsPreparer
    {
        public IEnumerable<string> PrepareTags(IEnumerable<string> tags)
        {
            return tags.Where(tag => tag.All(char.IsLetter));
        }
    }
}