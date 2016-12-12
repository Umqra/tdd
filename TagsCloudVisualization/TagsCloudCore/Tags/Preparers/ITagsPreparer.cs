using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudCore.Tags
{
    public interface ITagsPreparer
    {
        IEnumerable<string> PrepareTags(IEnumerable<string> tags);
    }
}
