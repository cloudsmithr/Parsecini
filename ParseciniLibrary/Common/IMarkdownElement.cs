using System;
namespace ParseciniLibrary.Common
{
    public interface IMarkdownElement : IElement
    {
        public string markdownSymbol { get; }
        public string htmlSymbol { get; }
        public void ParseTextFromMarkdown(string text);
        public void OutputHtmlFromText(string text);
    }
}
