using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseciniLibrary.Elements;
using ParseciniLibrary.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Unit.Tests.Templating
{
    [TestClass]
    public class TemplateTests
    {
        [TestMethod]
        public void TemplateWriteMeSuccess()
        {
            string result = GetValidTemplate().WriteMe();

            result.Should().Be(ExpectedResultString());
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
                            Content = "Header\n(tpl)[iconsTemplate.tpl](tpl)[metaTagsTemplate.tpl]"
                        });
            result.TemplateElements.Add("iconsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "iconsTemplate.tpl",
                            Content = "Icons\n"
                        });
            result.TemplateElements.Add("metaTagsTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "metaTagsTemplate.tpl",
                            Content = "META\n(tpl)[iconsTemplate.tpl]"
                        });
            result.TemplateElements.Add("bodyTemplate.tpl",
                        new TemplateElement()
                        {
                            FilePath = "bodyTemplate.tpl",
                            Content = "(CONTENT)"
                        });

            return result;
        }

        private string ExpectedResultString()
        {
            return "Header\nIcons\nMETA\nIcons\n(CONTENT)";
        }
    }
}
