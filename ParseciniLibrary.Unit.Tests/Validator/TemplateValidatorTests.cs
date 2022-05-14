using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseciniLibrary.Elements;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Templating;
using ParseciniLibrary.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Unit.Tests.Validator
{
    [TestClass]
    public class TemplateValidatorTests
    {

        [TestMethod]
        public void TemplateValidatorValidateSuccessful()
        {
            TemplateValidator validator = new TemplateValidator();

            bool result = validator.Validate(GetValidTemplate());

            result.Should().Be(true);
        }

        [TestMethod]
        public void TemplateValidatorValidateFailureSelfReference()
        {
            TemplateValidator validator = new TemplateValidator();

            var fn = () => validator.Validate(GetInvalidTemplateSelfReference());

            fn.Should().ThrowExactly<TemplateValidatorException>();
        }

        [TestMethod]
        public void TemplateValidatorValidateFailureCircularReference()
        {
            TemplateValidator validator = new TemplateValidator();

            var fn = () => validator.Validate(GetInvalidTemplateCircularReference());

            fn.Should().ThrowExactly<TemplateValidatorException>();
        }

        [TestMethod]
        public void TemplateValidatorValidateFailureCircularReference2()
        {
            TemplateValidator validator = new TemplateValidator();

            var fn = () => validator.Validate(GetInvalidTemplateCircularReference2());

            fn.Should().ThrowExactly<TemplateValidatorException>();
        }

        private Template GetInvalidTemplateCircularReference2()
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
                            Content = "(tpl)[iconsTemplate.tpl]"
                        });
            result.TemplateElements.Add("iconsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "iconsTemplate.tpl",
                            Content = "(tpl)[metaTagsTemplate.tpl]"
                        });
            result.TemplateElements.Add("metaTagsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "metaTagsTemplate.tpl",
                            Content = "META(tpl)[bodyTemplate.tpl]"
                        });
            result.TemplateElements.Add("bodyTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "bodyTemplate.tpl",
                            Content = "(CONTENT)(tpl)[headerTemplate.tpl]"
                        });

            return result;
        }

        private Template GetInvalidTemplateCircularReference()
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
                            Content = "META(tpl)[headerTemplate.tpl]"
                        });
            result.TemplateElements.Add("bodyTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "bodyTemplate.tpl",
                            Content = "(CONTENT)"
                        });

            return result;
        }

        private Template GetInvalidTemplateSelfReference()
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
                            Content = "(tpl)[iconsTemplate.tpl](tpl)[metaTagsTemplate.tpl](tpl)[headerTemplate.tpl]"
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


        private Template GetValidTemplate()
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
