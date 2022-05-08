using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using ParseciniLibrary.Common;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Parsing;
using System.IO;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Validator;
using ParseciniLibrary.Common.Parsing;

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
            if (Directory.Exists(path))
            {
                // We dealing with a directory
                string[] filenames = Directory.GetFiles(path);
                foreach(string filename in filenames)
                {
                    try
                    {
                        ProcessFile(filename);
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
                    ProcessFile(path);
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

        private void ProcessFile(string markdownFilePath)
        {
            Log.LogFile($"Beginning to process file: {markdownFilePath}");

            string markdownFileExtension = myConfig["FileSettings:MarkdownFileExtension"];

            MarkdownReader markdownReader = new MarkdownReader(myConfig, "MarkdownElements", new MarkdownElementValidator(new MarkdownTagValidator(), new HTMLTagValidator()));

            FileParser markdownFileParser = new FileParser(markdownFileExtension);
            IList<MarkdownElement> elements = ProcessObjectFile(markdownFilePath, markdownFileParser, markdownReader);

            // Now that we have the Markdown read in, we need to read in the rest of the theme information
            // Actually, this should probably be done on Initialization, so that the header / theme file etc. are only loaded once
            // and then applied to each Markdown file.

            //ThemeReader themeReader = new ThemeReader(myConfig, "Theme", new ThemeValidator());
            //IList<Themeelement> elements = ProcessObjectFile(filePath, fileParser, themeReader);
        }

        private IList<T> ProcessObjectFile<T>(string filePath, ITextParser fileParser, IObjectReader<T> objectReader)
        {
            List<string> results = fileParser.ParseFile(filePath);

            IList<T> readResults = objectReader.ReadObjectsFromStringList(results);

            return readResults;
        }

    }
}
