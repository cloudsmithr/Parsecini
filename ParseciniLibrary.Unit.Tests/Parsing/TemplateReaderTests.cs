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
            // TODO: build out the expected Template object and compare it to the result
            //Template expectedTemplate = new Template(new TemplateElement() { FilePath =  })

            FileParser testParser = new FileParser(".tpl");
            testParser.Should().NotBeNull();

            TemplateValidator templateValidator = new();


            TemplateReader templateReader = new TemplateReader(templateValidator, testParser);

            templateReader.Should().NotBeNull();

            Template resultTemplate = templateReader.ReadObjectFromString("\\TestData\\Themes\\Valid\\testTheme.tpl");

            resultTemplate.Should().NotBeNull();
        }




    }
}
