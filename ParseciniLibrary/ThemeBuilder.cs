using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using ParseciniLibrary.Common;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Parsing;
using System.IO;
using ParseciniLibrary.Exceptions;

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

            Log.LogPath = Path.Combine(Directory.GetCurrentDirectory(), myConfig["LogSettings:LogFolder"]);
            Log.LogFileName = myConfig["LogSettings:LogFileName"];
        }

        public void Process(string path, string fileExtension)
        {
            Log.BeginLogging();

            // We're using the passed-in file extension to tell the parser which files to parse.
            FileParser parser = new FileParser(fileExtension);

            if (Directory.Exists(path))
            {
                // We dealing with a directory
                string[] filenames = Directory.GetFiles(path);
                foreach(string filename in filenames)
                {
                    try
                    {
                        ProcessFile(filename, parser);
                    }
                    catch (Exception ex)
                    {
                        Log.LogFile(ex.ToString());
                    }
                }
            }
            else if (File.Exists(path))
            {
                // We're dealing with a file
                try
                {
                    ProcessFile(path, parser);
                }
                catch (Exception ex)
                {
                    Log.LogFile(ex.ToString());
                }
            }
            else
            {
                Log.EndLogging();
                throw new ThemeBuilderException("Could not find any files with the requested file extension at the given path.");
            }
            Log.EndLogging();
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
