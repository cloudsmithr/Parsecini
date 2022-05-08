using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Markdown;
using ParseciniLibrary.Parsing;
using ParseciniLibrary.Validator;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParseciniLibrary.Unit.Tests.Parsing
{
    [TestClass]
    public class MarkdownReaderTests
    {
        [TestMethod]
        [DynamicData(nameof(MarkdownReaderTestsData.MarkdownReaderReadObjectsSuccess), typeof(MarkdownReaderTestsData))]
        public void MarkdownReaderReadObjectsSuccess(string because, List<string> lines, List<MarkdownElement> expectedResults)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot myConfig = builder.Build();

            MarkdownReader markdownReader = new MarkdownReader(myConfig, "MarkdownElements", new MarkdownElementValidator(new MarkdownTagValidator(), new HTMLTagValidator()));

            IList<MarkdownElement> results = markdownReader.ReadObjectsFromStringList(lines);

            results.Should().NotBeNull(because);
            results.Should().BeEquivalentTo(expectedResults, because);
        }

        [TestMethod]
        [DynamicData(nameof(MarkdownReaderTestsData.MarkdownReaderReadObjectsFailure), typeof(MarkdownReaderTestsData))]
        public void MarkdownReaderReadObjectsFailure(string because, List<string> lines, string expectedErrorMessage)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot myConfig = builder.Build();

            MarkdownReader markdownReader = new MarkdownReader(myConfig, "MarkdownElements", new MarkdownElementValidator(new MarkdownTagValidator(), new HTMLTagValidator()));

            var fn = () => markdownReader.ReadObjectsFromStringList(lines);

            fn.Should().ThrowExactly<MarkdownReaderException>().WithMessage(expectedErrorMessage);
        }
    }
}
