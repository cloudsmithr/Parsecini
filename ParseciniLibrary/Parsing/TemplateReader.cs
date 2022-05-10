using Dawn;
using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Common.Parsing;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Elements;
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

        public TemplateReader(ITemplateValidator templateValidator)
        {

        }

        // Pass in the relative path to the root template file
        public Template ReadObjectFromString(string str)
        {
            return null;
        }

        private IList<string> ReadTemplatePathsFromStringList(List<string> stringList)
        {
            List<string> results = new List<string>();

            foreach(string str in stringList)
            {
                // Splitting the line by the (tpl) strings and only grabbing the ones that start with an opening bracket [
                IList<string> extractedStrings = str.Split("(tpl)", StringSplitOptions.RemoveEmptyEntries).ToList().Where(x => x[0] == '[').ToList();
                foreach (string s in extractedStrings)
                {
                    // split at the closing bracket
                    string fileNameSubString = s.Split(']')[0];
                    // Remove opening bracket
                    fileNameSubString = fileNameSubString.Remove(0, 1);

                    // A valid result will have at least x.tpl as characters. Anything less than this won't be valid.
                    if (fileNameSubString.Length < 5 || string.IsNullOrEmpty(fileNameSubString))
                    {
                        throw new TemplateReaderException("Invalid filename.");
                    }

                    if (new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileNameSubString)).Extension != ".tpl")
                    {
                        throw new TemplateReaderException("Invalid filename, file extension must be '.tpl'.");
                    }

                    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), fileNameSubString)))
                        results.Add(fileNameSubString);
                    else
                        throw new TemplateReaderException("Could not find the file specified.");
                }
            }

            return results;
        }
    }
}
