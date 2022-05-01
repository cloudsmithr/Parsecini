using System;
using System.Configuration;
using ParseciniLibrary.Common;

namespace ParseciniLibrary.MarkdownElements
{
    public class Header1 : IMarkdownElement
    {
        public string markdownSymbol { get; }
        public string htmlSymbol { get; }

        public void ParseTextFromMarkdown(string text)
        {

        }

        public void OutputHtmlFromText(string text)
        {

        }

        public Header1()
        {
            
        }
    }
}
