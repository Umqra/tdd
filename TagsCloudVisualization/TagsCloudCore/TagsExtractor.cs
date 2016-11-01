using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudCore
{
    public class TagsExtractor
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
        
        public IEnumerable<string> ExtractFromFile(string filename)
        {
            string text;
            using (var file = new StreamReader(File.Open(filename, FileMode.Open)))
            {
                text = file.ReadToEnd();
            }
            return
                Regex.Split(text, @"\b")
                    .Where(tag => tag.All(char.IsLetter) && tag.Length > 2)
                    .Select(tag => tag.ToLower())
                    .Where(tag => !stopWords.Contains(tag));
        }
    }
}