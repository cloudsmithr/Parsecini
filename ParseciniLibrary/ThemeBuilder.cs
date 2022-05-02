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

            // We have our MarkdownElements loaded from the config.
            MarkdownElement[] Elements = myConfig.GetSection("MarkdownElements").Get<MarkdownElement[]>();
        }

        public void Process(string path, string fileExtension)
        {
            FileParser parser = new FileParser(fileExtension);

            // 1. determine if we are processing a file or a directory
            // we're passing in a parser so the user can decide what they want to be processing, and set up their own processing options on said parser
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
            fileParser.ParseFile(filePath);
        }
    }
}
