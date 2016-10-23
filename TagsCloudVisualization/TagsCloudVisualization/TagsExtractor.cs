using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization
{
    public class TagsExtractor
    {
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
                    .Select(tag => tag.ToLower());
        }
    }
}