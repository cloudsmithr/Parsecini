using Dawn;
using ParseciniLibrary.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Parsing
{
    public class MarkdownWriter
    {
        private IList<MarkdownElement> markdownElements;
        private StringBuilder stringBuilder = new StringBuilder();

        public MarkdownWriter(IList<MarkdownElement> _markdownElements)
        {
            Guard.Argument(_markdownElements, nameof(_markdownElements)).NotNull().NotEmpty();
            markdownElements = _markdownElements;
        }

        public string WriteMe()
        {
            stringBuilder.Clear();

            foreach (MarkdownElement markdownElement in markdownElements)
            {
                if (markdownElement != null)
                {
                    if (markdownElement.htmlOpenSymbol.Contains("{Content}"))
                    {
                        stringBuilder.Append(markdownElement.htmlOpenSymbol.Replace("{Content}", markdownElement.Content));
                    }
                    else if (!string.IsNullOrWhiteSpace(markdownElement.htmlCloseSymbol))
                    {
                        stringBuilder.Append(markdownElement.htmlOpenSymbol);
                        stringBuilder.Append(markdownElement.Content);
                        stringBuilder.Append(markdownElement.htmlCloseSymbol);
                    }
                    else
                    {
                        stringBuilder.Append(markdownElement.htmlOpenSymbol);
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}
