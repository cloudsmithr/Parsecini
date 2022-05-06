using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Validator;
using System;
using System.Collections.Generic;
using System.IO;


namespace ParseciniLibrary.Unit.Tests.Validator
{
    [TestClass]
    public class MarkdownTagValidatorTests
    {
        [TestMethod]
        [DynamicData(nameof(MarkdownTagValidatorTestsData.MarkdownTagValidatorValidationSuccess), typeof(MarkdownTagValidatorTestsData))]
        public void MarkdownTagValidatorValidationSuccess(string markdownTag, bool isCloseSymbol = false)
        {
            TagValidatorMethod tagValidatorMethod = new TagValidatorMethod(markdownTag, isCloseSymbol);
            tagValidatorMethod.Should().NotBeNull();

            MarkdownTagValidator markdownTagValidator = new MarkdownTagValidator();
            markdownTagValidator.Should().NotBeNull();

            markdownTagValidator.Validate(tagValidatorMethod).Should().Be(true,
                markdownTag + ", " + isCloseSymbol.ToString());
        }

        [TestMethod]
        [DynamicData(nameof(MarkdownTagValidatorTestsData.MarkdownTagValidatorValidationFailure), typeof(MarkdownTagValidatorTestsData))]
        public void MarkdownTagValidatorValidationFailure(string markdownTag, bool isCloseSymbol = false)
        {
            TagValidatorMethod tagValidatorMethod = new TagValidatorMethod(markdownTag, isCloseSymbol);
            tagValidatorMethod.Should().NotBeNull();

            MarkdownTagValidator markdownTagValidator = new MarkdownTagValidator();
            markdownTagValidator.Should().NotBeNull();

            markdownTagValidator.Validate(tagValidatorMethod).Should().Be(false,
                markdownTag + ", " + isCloseSymbol.ToString());
        }
    }
}
