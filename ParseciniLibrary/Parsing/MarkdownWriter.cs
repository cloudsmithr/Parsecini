using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Parsing
{
    public class MarkdownWriter
    {
        IList<MarkdownElement> markdownElements;

        public MarkdownWriter(IList<MarkdownElement> _markdownElements)
        {
            markdownElements = _markdownElements;

        }
    }
}
