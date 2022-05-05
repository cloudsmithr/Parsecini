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
    public class HTMLTagValidatorTests
    {
        [TestMethod]
        [DynamicData(nameof(HTMLTagValidatorTestsData.HTMLTagValidatorValidationSuccess), typeof(HTMLTagValidatorTestsData))]
        public void HTMLTagValidatorValidationSuccess(string htmlCloseSymbol, bool isCloseSymbol = false)
        {
            TagValidatorMethod tagValidatorMethod = new TagValidatorMethod(htmlCloseSymbol, isCloseSymbol);
            tagValidatorMethod.Should().NotBeNull();

            HTMLTagValidator htmlTagValidator = new HTMLTagValidator();
            htmlTagValidator.Should().NotBeNull();

            htmlTagValidator.Validate(tagValidatorMethod).Should().Be(true);
        }

        [TestMethod]
        [DynamicData(nameof(HTMLTagValidatorTestsData.HTMLTagValidatorValidationFailure), typeof(HTMLTagValidatorTestsData))]
        public void HTMLTagValidatorValidationFailure(string htmlCloseSymbol, bool isCloseSymbol = false)
        {
            TagValidatorMethod tagValidatorMethod = new TagValidatorMethod(htmlCloseSymbol, isCloseSymbol);
            tagValidatorMethod.Should().NotBeNull();

            HTMLTagValidator htmlTagValidator = new HTMLTagValidator();
            htmlTagValidator.Should().NotBeNull();

            htmlTagValidator.Validate(tagValidatorMethod).Should().Be(false);
        }
    }
}
