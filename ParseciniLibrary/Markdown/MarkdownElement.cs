using ParseciniLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Markdown
{
    public class MarkdownElement : IMarkdownElement
    {
        public string name { get; set;  }
        public string markdownOpenSymbol { get; set;  }
        public string markdownCloseSymbol { get; set; }
        public string htmlOpenSymbol { get; set; }
        public string htmlCloseSymbol { get; set; }
        public string Content { get; set; }

        public string ReturnAsHtml()
        {
            return "";
        }

        public string ReturnAsMarkdown()
        {
            return "";
        }
    }
}
