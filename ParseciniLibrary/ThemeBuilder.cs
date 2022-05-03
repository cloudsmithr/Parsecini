using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using ParseciniLibrary.Common;
using ParseciniLibrary.Parsing;
using System.IO;

namespace ParseciniLibrary
{
    public class ThemeBuilder
    {
        public IConfigurationRoot myConfig { get; }

        public ThemeBuilder()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);
            myConfig = builder.Build();
        }

        public void Process(string path, string fileExtension)
        {
            // We're using the passed-in file extension to tell the parser which files to parse.
            FileParser parser = new FileParser(fileExtension);

            if (Directory.Exists(path))
            {
                // We dealing with a directory
                string[] filenames = Directory.GetFiles(path);
                foreach(string filename in filenames)
                {
                    ProcessFile(filename, parser);
                }
            }
            else if (File.Exists(path))
            {
                // We're dealing with a file
                ProcessFile(path, parser);
            }
            else
            {
                // Maybe throw an exception or message that we couldn't find anything.
            }
        }

        private void ProcessFile(string filePath, FileParser fileParser)
        {
            List<string> results = fileParser.ParseFile(filePath);

            MarkdownReader markdownReader = new MarkdownReader(myConfig, "MarkdownElements");

            MarkdownElement[] elements = markdownReader.ReadMarkdownElementsFromStringList(results);

            Console.WriteLine(results);
        }
    }
}
