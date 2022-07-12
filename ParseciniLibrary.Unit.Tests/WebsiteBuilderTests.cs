using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Parsing;
using System;
using System.Collections.Generic;
using System.IO;


namespace ParseciniLibrary.Unit.Tests
{
    [TestClass]
    public class WebsiteBuilderTests
    {
        [TestMethod]
        public void WebsiteBuilderConstructorSuccess()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData/TestWebsite1");
            WebsiteBuilder testWebsiteBuilder = new WebsiteBuilder(filePath);

            testWebsiteBuilder.Website.Name.Should().Be("TestWebsite");
            testWebsiteBuilder.Website.Author.Should().Be("Ryan Smith");
            testWebsiteBuilder.Website.Pages.Count.Should().Be(4);
            testWebsiteBuilder.Website.Pages[0].Title.Should().Be("home");
            testWebsiteBuilder.Website.Pages[0].Url.Should().Be("/");
            testWebsiteBuilder.Website.Pages[0].Template.Should().Be("/templates/home.tpl");
            testWebsiteBuilder.Website.Pages[0].Markdown.Should().Be("/home.mdt");
            testWebsiteBuilder.Website.Pages[3].Posts.Should().Be("/posts/");
            testWebsiteBuilder.Website.Pages[3].IsBlogPage.Should().Be(true);
            testWebsiteBuilder.Website.Pages[3].Pagination.Should().Be(5);
        }

        [TestMethod]
        public void WebsiteBuilderProcessSuccess()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData/TestWebsite1");
            WebsiteBuilder testWebsiteBuilder = new WebsiteBuilder(filePath);

            testWebsiteBuilder.ProcessSite().Should().Be(true);
        }
    }
}
