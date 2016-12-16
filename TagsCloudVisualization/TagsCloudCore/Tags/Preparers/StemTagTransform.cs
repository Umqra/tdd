using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using NHunspell;
using TagsCloudCore.Properties;

namespace TagsCloudCore.Tags.Preparers
{
    public class StemTagTransform : ITagsPreparer
    {
        public IEnumerable<string> PrepareTags(IEnumerable<string> tags)
        {
            using (var hunspell = new Hunspell(Resources.en_us_aff, Encoding.UTF8.GetBytes(Resources.en_us_dic)))
            {
                foreach (var tag in tags)
                {
                    var stems = hunspell.Stem(tag);
                    if (stems.Any())
                        yield return stems[0];
                    else
                        yield return tag;
                }
            }
        }
    }
}