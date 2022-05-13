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
    public class TemplateReaderTests
    {
        [TestMethod]
        public void TemplateReaderReadObjectFromStringSuccess()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();

            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            Template resultTemplate = templateReader.ReadObjectFromString("\\TestData\\Themes\\Valid\\testTheme.tpl");

            resultTemplate.Should().NotBeNull();
            resultTemplate.Should().BeEquivalentTo(GetExpectedTemplate());
        }

        [TestMethod]
        public void TemplateReaderReadObjectFromStringBrokenFileReference()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            var fn = () => templateReader.ReadObjectFromString("\\TestData\\Themes\\Invalid\\BrokenFileReference.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        [TestMethod]
        public void TemplateReaderReadObjectFromStringWrongFileExtension()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            var fn = () => templateReader.ReadObjectFromString("\\TestData\\Themes\\Invalid\\ReferenceWrongFileExtension.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        [TestMethod]
        public void TemplateReaderReadObjectFromStringInvalidFileName1()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            var fn = () => templateReader.ReadObjectFromString("\\TestData\\Themes\\Invalid\\InvalidFileName1.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        [TestMethod]
        public void TemplateReaderReadObjectFromStringInvalidFileName2()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            var fn = () => templateReader.ReadObjectFromString("\\TestData\\Themes\\Invalid\\InvalidFileName2.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        private Template GetExpectedTemplate()
        {
            TemplateElement rootElement = new TemplateElement()
            {
                FilePath = "\\TestData\\Themes\\Valid\\testTheme.tpl",
                Content = "(tpl)[/headerTemplate.tpl](tpl)[/bodyTemplate.tpl]"
            };

            Template result = new Template(rootElement);
            result.TemplateElements.Add("\\TestData\\Themes\\Valid\\headerTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "\\TestData\\Themes\\Valid\\headerTemplate.tpl",
                            Content = "(tpl)[iconsTemplate.tpl](tpl)[metaTagsTemplate.tpl]"
                        });
            result.TemplateElements.Add("\\TestData\\Themes\\Valid\\iconsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "\\TestData\\Themes\\Valid\\iconsTemplate.tpl",
                            Content = ""
                        });
            result.TemplateElements.Add("\\TestData\\Themes\\Valid\\metaTagsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "\\TestData\\Themes\\Valid\\metaTagsTemplate.tpl",
                            Content = "META"
                        });
            result.TemplateElements.Add("\\TestData\\Themes\\Valid\\bodyTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "\\TestData\\Themes\\Valid\\bodyTemplate.tpl",
                            Content = "(CONTENT)"
                        });

            return result;
        }

    }
}
