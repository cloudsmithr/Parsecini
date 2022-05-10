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
                    string originalFileNameSubString = s.Split(']')[0];

                    // Let's just strip out all of these characters that a user might have put at the beginning of the template path
                    while (s.Length > 0)
                    {
                        if (s[0] == '[' || s[0] == '.' || s[0] == '/' || s[0] == '\\' || s[0] == ' ' || s[0] == '\n' || s[0] == '\r' || s[0] == '\'' || s[0] == '\"')
                            s.Remove(0, 1);
                        else
                            break;
                    }

                    // A valid result will have at least x.tpl] as characters. Anything less than this won't be valid.
                    if(s.Length < 6 || string.IsNullOrEmpty(s))
                    {
                        throw new TemplateReaderException("Invalid filename.");
                    }

                    // We split the string again on the closing bracket, taking only the first substring since that will be our filename.
                    string fileNameSubString = s.Split(']')[0];

                    if (fileNameSubString != originalFileNameSubString)
                    {
                        throw new TemplateReaderException($"Invalid format, please correct it from [{originalFileNameSubString}] to: [{fileNameSubString}]");
                    }

                    while (fileNameSubString.Length > 0)
                    {
                        char lastCharacter = fileNameSubString[fileNameSubString.Length - 1];
                        if (lastCharacter == '[' || lastCharacter == '.' ||
                            lastCharacter == '/' || lastCharacter == '\\' ||
                            lastCharacter == ' ' || lastCharacter == '\n' ||
                            lastCharacter == '\r' || lastCharacter == '\'' ||
                            lastCharacter == '\"')
                            fileNameSubString.Remove(fileNameSubString.Length - 1, 1);
                        else
                            break;
                    }

                    if (fileNameSubString.Substring(fileNameSubString.Length - 4) != ".tpl")
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
