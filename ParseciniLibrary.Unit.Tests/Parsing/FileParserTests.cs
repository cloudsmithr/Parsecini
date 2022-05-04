using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Parsing;
using System;
using System.Collections.Generic;
using System.IO;


namespace ParseciniLibrary.Unit.Tests.Parsing
{
    [TestClass]
    public class FileParserTests
    {
        [TestMethod]
        public void FileParsereConstructorSuccess()
        {
            FileParser testParser = new FileParser(".test");
            testParser.Should().NotBeNull();
            testParser.fileExtension.Should().Be(".test");
        }

        [TestMethod]
        public void FileParserConstructorSuccess2()
        {
            FileParser testParser = new FileParser("test");
            testParser.Should().NotBeNull();
            testParser.fileExtension.Should().Be(".test");
        }

        [TestMethod]
        public void FileParserParseDirectoryNoFilePath()
        {
            string dirPath = "";
            FileParser fileParser = new FileParser("mdt");

            var fn = () => fileParser.ParseFile(dirPath);
            fn.Should().ThrowExactly<ArgumentException>().WithParameterName("filePath");
        }

        [TestMethod]
        public void FileParserParseFileNotExist()
        {
            string dirPath = Directory.GetCurrentDirectory() + "/TestData/fakeFile.mdt";
            FileParser fileParser = new FileParser("mdt");

            var fn = () => fileParser.ParseFile(dirPath);
            fn.Should().Throw<FileParserException>().WithMessage($"Cannot parse file {dirPath} as the file does not exist.");
        }

        [TestMethod]
        public void FileParserParseFileWrongExtension()
        {
            string dirPath = Directory.GetCurrentDirectory() + "/TestData/test01.mdt";
            string fileExtension = ".txt";

            FileParser fileParser = new FileParser(fileExtension);

            var fn = () => fileParser.ParseFile(dirPath);
            fn.Should().Throw<FileParserException>().WithMessage($"The filePath {dirPath} does not end in the extension '{fileExtension}' assigned to this FileParser.");
        }

        [TestMethod]
        public void FileParserParseFileSuccess()
        {
            string dirPath = Directory.GetCurrentDirectory() + "/TestData/test01.mdt";
            FileParser fileParser = new FileParser("mdt");

            List<string> results = fileParser.ParseFile(dirPath);
            results.Should().BeEquivalentTo(testFileEquivalent);
        }

        private List<string> testFileEquivalent = new List<string>
        {
            "# Header! Look at me",
            "[text]",
            "And now I will write a paragraph. A simple little paragraph!",
            "Even with a newline!",
            "[/text]"
        };
    }
}