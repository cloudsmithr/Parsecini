﻿using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Elements;
using System;
using System.Collections.Generic;
using ParseciniLibrary.Common;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Parsing;
using ParseciniLibrary.Templating;
using System.IO;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Validator;
using ParseciniLibrary.Common.Parsing;
using System.Text;

namespace ParseciniLibrary
{
    public class ThemeBuilder
    {
        public IConfigurationRoot myConfig { get; }
        private Template Template;

        public ThemeBuilder()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);
            myConfig = builder.Build();

            Log.LogPath = Path.Combine(Directory.GetCurrentDirectory(), myConfig["LogSettings:LogFolder"]);
            Log.LogFileName = myConfig["LogSettings:LogFileName"];
        }

        public void SetTemplate(string rootTemplateAbsolutePath)
        {
            Log.BeginLogging();

            if (File.Exists(rootTemplateAbsolutePath))
            {
                try
                {
                    ProcessTemplateFile(rootTemplateAbsolutePath);
                }
                catch (Exception ex)
                {
                    Log.LogFile($"There was an error parsing the template file. The error found was: {ex}");
                    Log.EndLogging();
                    throw;
                }
            }
            else
            {
                Log.EndLogging();
                throw new ThemeBuilderException("Could not find any files with the requested file extension at the given path.");
            }
            Log.EndLogging();
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
                        ProcessMarkdownFile(filename);
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
                    ProcessMarkdownFile(path);
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

        private void ProcessTemplateFile(string templateFileAbsolutePath)
        {
            Log.LogFile($"Beginning to process file: {templateFileAbsolutePath}");

            string templateFileExtension = myConfig["FileSettings:TemplateFileExtension"];

            TemplateReader templateReader = new TemplateReader(new TemplateValidator(), new FileParser(templateFileExtension), templateFileExtension);
            Template = templateReader.ReadObjectFromString(templateFileAbsolutePath);
        }

        private void ProcessMarkdownFile(string markdownFilePath)
        {
            if (Template == null)
                throw new Exception("Cannot process a MarkdownFile without a valid Template set.");

            Log.LogFile($"Beginning to process file: {markdownFilePath}");

            string markdownFileExtension = myConfig["FileSettings:MarkdownFileExtension"];

            MarkdownReader markdownReader = new MarkdownReader(myConfig, "MarkdownElements", new MarkdownElementValidator(new MarkdownTagValidator(), new HTMLTagValidator()));

            FileParser markdownFileParser = new FileParser(markdownFileExtension);
            WriteMarkdownToTemplate(ProcessObjectListFile(markdownFilePath, markdownFileParser, markdownReader));
        }

        private void WriteMarkdownToTemplate(IList<MarkdownElement> markdownElements)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(Template.WriteMe());


        }

        private IList<T> ProcessObjectListFile<T>(string filePath, ITextParser fileParser, IObjectListReader<T> objectReader)
        {
            List<string> results = fileParser.ParseFile(filePath);

            IList<T> readResults = objectReader.ReadObjectsFromStringList(results);

            return readResults;
        }

    }
}
