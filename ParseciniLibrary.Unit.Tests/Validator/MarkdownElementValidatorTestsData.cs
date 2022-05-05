using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Unit.Tests.Validator
{
    public class MarkdownElementValidatorTestsData
    {
        public static IEnumerable<object[]> MarkdownElementValidatorValidationSuccess
        {
            get
            {
                return new[]
                {
                    new object[] { new MarkdownElement() 
                    {
                        markdownOpenSymbol = "[text]",
                        markdownCloseSymbol = "[/text]",
                        htmlOpenSymbol = "<p>",
                        htmlCloseSymbol = "</p>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "#",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = "</h1>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "-",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<img {content} />",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "+",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<br />",
                        htmlCloseSymbol = ""
                    } },
                };
            }
        }

        public static IEnumerable<object[]> MarkdownElementValidatorValidationFailure
        {
            get
            {
                return new[]
                {
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "+",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "+",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "-",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = "-"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "[test]",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "[/test]",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = "</h1>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "[test]",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = "</h1>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "[/test]",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "[test]",
                        markdownCloseSymbol = "[/test]",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = "</h1>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "+",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = ""
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "[test]",
                        markdownCloseSymbol = "[/test]",
                        htmlOpenSymbol = "",
                        htmlCloseSymbol = "</h1>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "",
                        markdownCloseSymbol = "[/test]",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = "</h1>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "+",
                        markdownCloseSymbol = "[/test]",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = "</h1>"
                    } },
                    new object[] { new MarkdownElement()
                    {
                        markdownOpenSymbol = "[test]",
                        markdownCloseSymbol = "+",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = "</h1>"
                    } },
                };
            }
        }
    }
}
