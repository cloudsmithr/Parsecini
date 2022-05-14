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

            Template resultTemplate = templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Valid\\testTheme.tpl");

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

            var fn = () => templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Invalid\\BrokenFileReference.tpl");
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

            var fn = () => templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Invalid\\ReferenceWrongFileExtension.tpl");
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

            var fn = () => templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Invalid\\InvalidFileName1.tpl");
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

            var fn = () => templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Invalid\\InvalidFileName2.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        [TestMethod]
        public void TemplateReaderReadObjectFromStringInvalidFileName3()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            var fn = () => templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Invalid\\InvalidFileName3.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        [TestMethod]
        public void TemplateReaderReadObjectFromStringInvalidFileName4()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            var fn = () => templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Invalid\\InvalidFileName4.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        [TestMethod]
        public void TemplateReaderReadObjectFromStringInvalidFileName5()
        {
            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser, ".tpl");

            templateReader.Should().NotBeNull();

            var fn = () => templateReader.ReadObjectFromString(Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Invalid\\InvalidFileName5.tpl");
            fn.Should().ThrowExactly<TemplateReaderException>();
        }

        private Template GetExpectedTemplate()
        {
            TemplateElement rootElement = new TemplateElement()
            {
                FilePath = "testTheme.tpl",
                Content = "(tpl)[headerTemplate.tpl](tpl)[bodyTemplate.tpl]"
            };

            Template result = new Template(rootElement, Directory.GetCurrentDirectory() + "\\TestData\\Themes\\Valid\\testTheme.tpl");
            result.TemplateElements.Add("headerTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "headerTemplate.tpl",
                            Content = "(tpl)[iconsTemplate.tpl](tpl)[metaTagsTemplate.tpl]"
                        });
            result.TemplateElements.Add("iconsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "iconsTemplate.tpl",
                            Content = ""
                        });
            result.TemplateElements.Add("metaTagsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "metaTagsTemplate.tpl",
                            Content = "META"
                        });
            result.TemplateElements.Add("bodyTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "bodyTemplate.tpl",
                            Content = "(CONTENT)"
                        });

            return result;
        }

    }
}
