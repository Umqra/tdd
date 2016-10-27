using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudCore
{
    public class TagsExtractor
    {
        // It's usually called stop-words
        // CR: Why it would be public?
        public static string[] AuxiliaryWords =
        {
            "the",
            "and",
            "that",
            "this",
            "then",
            "than",
            "are",
            "which",
            "where",
            "what",
            "for",
            "was",
            "not",
            "there"
        };
        // CR: Right here should be a new line
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
                    .Where(tag => !AuxiliaryWords.Contains(tag));
        }
    }
}