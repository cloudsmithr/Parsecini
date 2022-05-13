using Dawn;
using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Common.Parsing;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Elements;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ParseciniLibrary.Templating;

namespace ParseciniLibrary.Parsing
{
    public class TemplateReader : IObjectReader<Template>
    {
        public Template _object { get; set; }
        private ITextParser textParser { get; }
        private ITemplateValidator templateValidator { get; }
        private string templateFileExtension;

        public TemplateReader(ITemplateValidator _templateValidator, ITextParser _textParser, string _templateFileExtension)
        {
            Guard.Argument(_templateValidator, nameof(_templateValidator)).NotNull();
            Guard.Argument(_textParser, nameof(_textParser)).NotNull();
            Guard.Argument(_templateFileExtension, nameof(_templateFileExtension)).NotNull().NotWhiteSpace();

            textParser = _textParser;
            templateValidator = _templateValidator;
            templateFileExtension = _templateFileExtension.Replace(".", string.Empty);
        }

        // Pass in the path to the root template file
        public Template ReadObjectFromString(string str)
        {
            ReadTemplateElementsRecursively(str, str);

            if (templateValidator.Validate(_object))
                return _object;
            else
                throw new TemplateReaderException("");
        }

        private void ReadTemplateElementsRecursively(string filePath, string originalFile)
        {
            if (filePath.Contains(Directory.GetCurrentDirectory()))
                filePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

            List<string> templateLines = new List<string>();

            // If something goes wrong while reading in the file we need to be sure to tell the user what template has the problem.
            try
            {
                templateLines = textParser.ParseFile(filePath);
            }
            catch (Exception ex)
            {
                throw new TemplateReaderException($"Encountered file parsing error in the template file {originalFile}. Could not parse file path {filePath}", ex);
            }

            // Make sure the template isn't referencing itself
            foreach (string line in templateLines)
            {
                if(line.Contains(filePath))
                {
                    throw new TemplateReaderException($"Self-reference detected in {filePath}.");
                }
            }

            // If we haven't already added this template to the Template _object, we need to add it and search it for children.
            // If it's already in the list we don't have to do anything.
            if (!(_object is object) || !_object.TemplateElements.ContainsKey(filePath))
            {
                TemplateElement templateElement = new TemplateElement()
                { FilePath = filePath, Content = textParser.JoinString(Environment.NewLine, templateLines) };

                if (!(_object is object))
                    _object = new Template(templateElement);
                else
                    _object.TemplateElements.Add(filePath, templateElement);

                IList<string> getChildren = ReadTemplatePathsFromStringList(templateLines, filePath);

                // If we have children, we have to go through them recursively as well.
                if(getChildren.Count > 0)
                {
                    foreach(string childTemplate in getChildren)
                    {
                        ReadTemplateElementsRecursively(childTemplate, filePath);
                    }
                }
            }
        }

        private IList<string> ReadTemplatePathsFromStringList(List<string> stringList, string filePath)
        {
            List<string> results = new List<string>();
            string fileDirectory = Path.GetDirectoryName(filePath);

            foreach (string str in stringList)
            {
                // Splitting the line by the ({templateFileExtension}) strings and only grabbing the ones that start with an opening bracket [
                IList<string> extractedStrings = str.Split($"({templateFileExtension})", StringSplitOptions.RemoveEmptyEntries).ToList().Where(x => x[0] == '[').ToList();
                foreach (string s in extractedStrings)
                {
                    // split at the closing bracket
                    string fileNameSubString = s.Split(']')[0];
                    // Remove opening bracket
                    fileNameSubString = fileNameSubString.Remove(0, 1);

                    // strip out any leading back or forward slashes
                    while (fileNameSubString.Length > 0 && (fileNameSubString[0] == '\\' || fileNameSubString[0] == '/'))
                        fileNameSubString = fileNameSubString.Remove(0, 1);

                    // A valid result will have at least x.{fileExtension} as characters. Anything less than this won't be valid.
                    if (fileNameSubString.Length < 2 + templateFileExtension.Length || string.IsNullOrEmpty(fileNameSubString))
                    {
                        throw new TemplateReaderException($"Invalid filename {fileNameSubString} in {filePath}.");
                    }

                    if (!fileNameSubString.Contains(fileDirectory))
                        fileNameSubString = Path.Join(fileDirectory, fileNameSubString);

                    string fullFilePath = Path.Join(Directory.GetCurrentDirectory(), fileNameSubString);

                    if (new FileInfo(fullFilePath).Extension != $".{templateFileExtension}")
                    {
                        throw new TemplateReaderException($"Invalid filename {fullFilePath} in {filePath}, file extension must be '.{templateFileExtension}'.");
                    }

                    if (File.Exists(fullFilePath))
                        results.Add(fileNameSubString);
                    else
                        throw new TemplateReaderException($"Could not find the template file {fullFilePath}, linked in {filePath}.");
                }
            }

            return results;
        }
    }
}
