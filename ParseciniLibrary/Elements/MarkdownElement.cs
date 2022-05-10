using ParseciniLibrary.Common;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Common.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Elements
{
    public class MarkdownElement : IMarkdownElement, IValidatorMethod, ICloneable<MarkdownElement>
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

        public MarkdownElement Clone()
        {
            MarkdownElement element = new MarkdownElement();
            element.name = name;
            element.markdownOpenSymbol = markdownOpenSymbol;
            element.markdownCloseSymbol = markdownCloseSymbol;
            element.htmlOpenSymbol = htmlOpenSymbol;
            element.htmlCloseSymbol = htmlCloseSymbol;
            element.Content = Content;

            return element;
        }
    }
}
