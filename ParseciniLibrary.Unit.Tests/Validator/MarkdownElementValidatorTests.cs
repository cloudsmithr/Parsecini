using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParseciniLibrary.Common;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Markdown;
using ParseciniLibrary.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParseciniLibrary.Unit.Tests.Validator
{
    [TestClass]
    public class MarkdownElementValidatorTests
    {
        [TestMethod]
        [DynamicData(nameof(MarkdownElementValidatorTestsData.MarkdownElementValidatorValidationSuccess), typeof(MarkdownElementValidatorTestsData))]
        public void MarkdownTagValidatorValidationSuccess(MarkdownElement markdownElement)
        {
            Mock<IMarkdownTagValidator> mockMarkdownTagValidator = new Mock<IMarkdownTagValidator>();
            Mock<IHTMLTagValidator> mockHTMLTagValidator = new Mock<IHTMLTagValidator>();

            mockHTMLTagValidator.Setup(x => x.Validate(It.IsAny<TagValidatorMethod>())).Returns(true);
            mockMarkdownTagValidator.Setup(foo => foo.Validate(It.IsAny<TagValidatorMethod>())).Returns(true);

            MarkdownElementValidator markdownElementValidator = new MarkdownElementValidator(mockMarkdownTagValidator.Object, mockHTMLTagValidator.Object);
            markdownElementValidator.Should().NotBeNull();

            markdownElementValidator.Validate(markdownElement).Should().Be(true, 
                markdownElement.markdownOpenSymbol + "," +
                markdownElement.markdownCloseSymbol + "," +
                markdownElement.htmlOpenSymbol + "," +
                markdownElement.htmlCloseSymbol);
        }

        [TestMethod]
        [DynamicData(nameof(MarkdownElementValidatorTestsData.MarkdownElementValidatorValidationFailure), typeof(MarkdownElementValidatorTestsData))]
        public void MarkdownTagValidatorValidationFailure(MarkdownElement markdownElement)
        {
            Mock<IMarkdownTagValidator> mockMarkdownTagValidator = new Mock<IMarkdownTagValidator>();
            Mock<IHTMLTagValidator> mockHTMLTagValidator = new Mock<IHTMLTagValidator>();

            mockMarkdownTagValidator.Setup(x => x.Validate(It.IsAny<TagValidatorMethod>())).Returns(true);
            mockHTMLTagValidator.Setup(x => x.Validate(It.IsAny<TagValidatorMethod>())).Returns(true);

            MarkdownElementValidator markdownElementValidator = new MarkdownElementValidator(mockMarkdownTagValidator.Object, mockHTMLTagValidator.Object);
            markdownElementValidator.Should().NotBeNull();

            markdownElementValidator.Validate(markdownElement).Should().Be(false,
                markdownElement.markdownOpenSymbol + "," +
                markdownElement.markdownCloseSymbol + "," +
                markdownElement.htmlOpenSymbol + "," +
                markdownElement.htmlCloseSymbol);
        }
    }
}
