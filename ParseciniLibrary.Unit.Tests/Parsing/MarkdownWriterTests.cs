using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseciniLibrary.Elements;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Parsing;
using ParseciniLibrary.Templating;
using ParseciniLibrary.Validator;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParseciniLibrary.Unit.Tests.Parsing
{
    [TestClass]
    public class MarkdownWriterTests
    {
        [TestMethod]
        public void MarkdownWriterSuccess()
        {
            List<MarkdownElement> markdownElementList = new List<MarkdownElement>
                {
                    new MarkdownElement
                    {
                        name = "Header1",
                        markdownOpenSymbol = "#",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<h1>",
                        htmlCloseSymbol = "</h1>",
                        Content = "Sup"
                    },
                    new MarkdownElement
                    {
                        name = "Paragraph1",
                        markdownOpenSymbol = "[text]",
                        markdownCloseSymbol = "[/text]",
                        htmlOpenSymbol = "<p>",
                        htmlCloseSymbol = "</p>",
                        Content = "Hello! Tis me, I am the one!"
                    },
                    new MarkdownElement
                    {
                        name = "Breakline",
                        markdownOpenSymbol = "-",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<br />",
                        htmlCloseSymbol = "",
                        Content = ""
                    },
                    new MarkdownElement
                    {
                        name = "Image1",
                        markdownOpenSymbol = "+",
                        markdownCloseSymbol = "",
                        htmlOpenSymbol = "<img {Content} />",
                        htmlCloseSymbol = "",
                        Content = "src='https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/Golde33443.jpg/220px-Golde33443.jpg'"
                    }
                };

            MarkdownWriter markdownWriter = new MarkdownWriter(markdownElementList);

            string results = markdownWriter.WriteMe();

            results.Should().NotBeNullOrEmpty();
            results.Should().Be("<h1>Sup</h1><p>Hello! Tis me, I am the one!</p><br /><img src='https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/Golde33443.jpg/220px-Golde33443.jpg' />");
        }
    }
}
