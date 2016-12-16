using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;

namespace TagsCloudCore.Tags
{
    public class DocxLineExtractor : ILinesExtractor
    {
        public IEnumerable<string> ExtractLines(StreamReader stream)
        {
            using (var document = WordprocessingDocument.Open(stream.BaseStream, false))
            {
                return new [] {document.MainDocumentPart.Document.Body.InnerText};
            }
        }
    }
}
