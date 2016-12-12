using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudCore.Tags
{
    public class StopWordsFilter : ITagsPreparer
    {
        public List<string> StopWords { get; set; }
        public StopWordsFilter()
        {
            StopWords = new List<string>();
            LoadStopWordsInternal();
        }

        private void LoadStopWordsInternal()
        {
            //TODO: Move this into App.Settings
            StopWords = new List<string>
            {
                "the", "and", "that", "this", "then",
                "than", "are", "which", "where", "what",
                "for", "was", "not", "there", "did",
                "his", "you", "him", "didn", "had", "their",
                "they", "them", "from", "could", "were",
                "but", "with", "when", "have", "would", "its",
                "should", "who", "been", "be", "she",
                "her", "he", "don", "said", "has",
                "can", "some", "one", "into", "just"
            };
        }

        public StopWordsFilter(string filename)
        {
            LoadStopWordsFromFile(filename);
        }

        private void LoadStopWordsFromFile(string filename)
        {
            using (var stream = new StreamReader(File.OpenRead(filename)))
            {
                while (!stream.EndOfStream)
                {
                    var word = stream.ReadLine();
                    if (word != null && word.Trim() != "")
                        StopWords.Add(word.Trim());
                }
            }
        }
        public IEnumerable<string> PrepareTags(IEnumerable<string> tags)
        {
            return tags.Where(tag => tag.Length > 2 && !StopWords.Contains(tag));
        }
    }
}