using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudCore.Tags
{
    public class TagsPreparer : ITagsPreparer
    {
        //TODO: Stop words list in settings
        private static readonly string[] stopWords =
        {
            "the", "and", "that", "this", "then", "than", "are",
            "which", "where", "what", "for", "was", "not", "there",
            "did", "his", "you", "him", "didn", "had", "their", "they",
            "them", "from", "could", "were", "but", "with",
            "when", "have", "would", "its", "should", "who",
            "been", "be", "she", "her", "he", "don", "said",
            "has", "can", "some", "one", "into", "just" 
        };

        private IEnumerable<string> SplitLineByTokens(string line)
        {
            return Regex.Split(line, @"\b");
        }

        public IEnumerable<string> PrepareTags(IEnumerable<string> lines)
        {
            return lines.SelectMany(SplitLineByTokens)
                .Where(tag => tag.All(char.IsLetter) && tag.Length > 2)
                .Select(tag => tag.ToLower())
                .Where(tag => !stopWords.Contains(tag));
        }
    }
}