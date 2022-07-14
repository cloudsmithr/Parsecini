using Microsoft.Extensions.Configuration;
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
using Dawn;

namespace ParseciniLibrary
{
    public class ThemeBuilder
    {
        public IConfigurationRoot myConfig { get; }
        private Template Template;
        private string OutputFolder = "";
        private string OriginalFileName = "";
        private string OriginalFileExtension = "";
        public Dictionary<string, string> WebsiteVariables = new Dictionary<string, string>();
        private Dictionary<string, string> TemplateVariables = new Dictionary<string, string>();
        private Dictionary<string, string> PostVariables;

        public ThemeBuilder(IConfigurationRoot config)
        {
            Guard.Argument(config, nameof(config)).NotNull();
            myConfig = config;
        }

        public void SetTemplate(string rootTemplateAbsolutePath)
        {

            if (File.Exists(rootTemplateAbsolutePath))
            {
                try
                {
                    Template = ProcessTemplateFile(rootTemplateAbsolutePath);
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

        public void SetOutputFolder(string outputFolder)
        {
            if(!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            OutputFolder = outputFolder;
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

        private Template ProcessTemplateFile(string templateFileAbsolutePath)
        {
            Log.LogFile($"Beginning to process file: {templateFileAbsolutePath}");

            string templateFileExtension = myConfig["FileSettings:TemplateFileExtension"];

            TemplateReader templateReader = new TemplateReader(new TemplateValidator(), new FileParser(templateFileExtension), templateFileExtension);
            return templateReader.ReadObjectFromString(templateFileAbsolutePath);
        }

        private void ProcessMarkdownFile(string markdownFilePath)
        {
            OriginalFileName = Path.GetFileName(markdownFilePath);
            OriginalFileExtension = Path.GetExtension(markdownFilePath);

            if (Template == null)
                throw new Exception("Cannot process a MarkdownFile without a valid Template set.");

            Log.LogFile($"Beginning to process file: {markdownFilePath}");

            string markdownFileExtension = myConfig["FileSettings:MarkdownFileExtension"];

            MarkdownReader markdownReader = new MarkdownReader(myConfig, "MarkdownElements", new MarkdownElementValidator(new MarkdownTagValidator(), new HTMLTagValidator()));

            FileParser markdownFileParser = new FileParser(markdownFileExtension);
            List<string> results = markdownFileParser.ParseFile(markdownFilePath);
            List<string> clearedList = new List<string>();
            
            TemplateVariables.Clear();

            foreach (string s in results)
            {
                if (s.StartsWith("{var}"))
                {
                    string substring = s.Remove(0, 5);
                    substring = substring.Replace("{", "").Replace("}", "");
                    string[] holder = substring.Split('=');
                    TemplateVariables.Add(holder[0], holder[1]);
                }
                else if (s.StartsWith("{prv}") && PostVariables is object)
                {
                    string substring = s.Remove(0, 5);
                    substring = substring.Replace("{", "").Replace("}", "");
                    string[] holder = substring.Split('=');
                    PostVariables.Add(holder[0], holder[1]);
                }
                else
                {
                    clearedList.Add(s);
                }
            }

            WriteMarkdownToTemplate(ProcessObjectListFile(clearedList, markdownReader));
        }

        private async void WriteMarkdownToTemplate(IList<MarkdownElement> markdownElements)
        {
            StringBuilder stringBuilder = new StringBuilder();
            MarkdownWriter markdownWriter = new MarkdownWriter(markdownElements);

            // We assemble our template
            stringBuilder.Append(Template.WriteMe());
            // Then replace the Content string with the content of our markdown file
            stringBuilder.Replace("(Content)", markdownWriter.WriteMe());

            foreach (KeyValuePair<string, string> s in WebsiteVariables)
            {
                stringBuilder.Replace($"{{{s.Key}}}", s.Value);
            }

            foreach (KeyValuePair<string, string> s in TemplateVariables)
            {
                stringBuilder.Replace($"{{{s.Key}}}", s.Value);
            }

            await File.WriteAllTextAsync(Path.Join(OutputFolder, OriginalFileName.Replace(OriginalFileExtension, ".html")), stringBuilder.ToString());
        }

        public async void WriteBlogIndexToTemplate(BlogIndex blogIndex, string path)
        {
            // TODO: This needs support for pagination and maybe tags
            Template mainTemplate = ProcessTemplateFile(blogIndex.TemplatePath);
            Template previewTemplate = ProcessTemplateFile(blogIndex.PreviewTemplatePath);

            int numPages = (int)Math.Ceiling((float)blogIndex.Posts.Count / (float)blogIndex.PostsPerPage);

            for (int i = 0; i < numPages; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(mainTemplate.WriteMe());

                foreach (KeyValuePair<string, string> s in WebsiteVariables)
                {
                    stringBuilder.Replace($"{{{s.Key}}}", s.Value);
                }

                foreach (KeyValuePair<string, string> s in TemplateVariables)
                {
                    stringBuilder.Replace($"{{{s.Key}}}", s.Value);
                }

                StringBuilder contentBuilder = new StringBuilder();

                //foreach (Post post in blogIndex.Posts)
                for(int j = blogIndex.PostsPerPage * i; j < blogIndex.PostsPerPage * (i + 1); j++)
                {
                    if (j < blogIndex.Posts.Count)
                    {
                        string previewString = previewTemplate.WriteMe();

                        foreach (KeyValuePair<string, string> s in blogIndex.Posts[j].Variables)
                        {
                            previewString = previewString.Replace($"{{{s.Key}}}", s.Value);
                        }

                        contentBuilder.Append(previewString);
                    }
                }

                if(numPages > 1)
                {
                    if (i == 0)
                    {
                        contentBuilder.Append(blogIndex.BlogNavigation.WriteMe(
                            nextPageUrl: blogIndex.IndexUrl(i + 2),
                            lastPageUrl: blogIndex.IndexUrl(numPages)));
                    }
                    else if (i == numPages - 1)
                    {
                        contentBuilder.Append(blogIndex.BlogNavigation.WriteMe(
                            firstPageUrl: blogIndex.Url,
                            prevPageUrl: blogIndex.IndexUrl(numPages - 1)));
                    }
                    else if (i - 1 == 0)
                    {
                        contentBuilder.Append(blogIndex.BlogNavigation.WriteMe(
                            firstPageUrl: blogIndex.Url,
                            prevPageUrl: blogIndex.Url,
                            nextPageUrl: blogIndex.IndexUrl(i + 2),
                            lastPageUrl: blogIndex.IndexUrl(numPages)));
                    }
                    else
                    {
                        contentBuilder.Append(blogIndex.BlogNavigation.WriteMe(
                            firstPageUrl: blogIndex.Url,
                            prevPageUrl: blogIndex.IndexUrl(i),
                            nextPageUrl: blogIndex.IndexUrl(i + 2),
                            lastPageUrl: blogIndex.IndexUrl(numPages)));
                    }
                }

                stringBuilder.Replace("(Content)", contentBuilder.ToString());
                if (i == 0)
                    await File.WriteAllTextAsync(Path.Join(OutputFolder, path), stringBuilder.ToString());
                else
                    await File.WriteAllTextAsync(Path.Join(OutputFolder, path.Replace(".html", $"{i+1}.html")), stringBuilder.ToString());
            }
        }

        public void SetPostVariableDictionary(Dictionary<string, string> variableDictionary)
        {
            PostVariables = variableDictionary;
        }

        private IList<T> ProcessObjectListFile<T>(List<string> lines, IObjectListReader<T> objectReader)
        {
            IList<T> readResults = objectReader.ReadObjectsFromStringList(lines);

            return readResults;
        }

    }
}
