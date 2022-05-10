using System;
namespace ParseciniLibrary.Common.Elements
{
    public interface IMarkdownElement : IElement
    {
        public string name { get; }
        public string markdownOpenSymbol { get; }
        public string markdownCloseSymbol { get; }
        public string htmlOpenSymbol { get; }
        public string htmlCloseSymbol { get; }

        public string Content { get; set; }
        public string ReturnAsHtml();
        public string ReturnAsMarkdown();

    }
}
