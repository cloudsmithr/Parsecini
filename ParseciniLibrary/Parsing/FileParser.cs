using Dawn;
using ParseciniLibrary.Common.Parsing;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParseciniLibrary.Parsing
{
    public class FileParser : ITextParser
    {
        public string fileExtension { get; set; } 
        public FileParser(string _fileExtension)
        {
            Log.LogFile($"Building Fileparser for file extension {_fileExtension}");

            Guard.Argument(_fileExtension, nameof(_fileExtension)).NotNull().NotWhiteSpace();

            if (!_fileExtension.StartsWith('.'))
                _fileExtension = "." + _fileExtension;

            fileExtension = _fileExtension;
        }

        public List<string> ParseFile(string filePath)
        {
            Guard.Argument(filePath, nameof(filePath)).NotNull().NotWhiteSpace();

            if (File.Exists(filePath))
            {
                if (Path.GetExtension(filePath) == fileExtension)
                {
                    try
                    {
                        // It's a File, and it exists, and has the right fileExtension we can read it.
                        return GrabLines(filePath);
                    }
                    catch(Exception e)
                    {
                        throw new FileParserException($"Something went wrong while reading the file {filePath}. Please check the disk and try again.", e);
                    }
                }
                else
                {
                    throw new FileParserException($"The filePath {filePath} does not end in the extension '{fileExtension}' assigned to this FileParser.");
                }
            }
            else
            {
                throw new FileParserException($"Cannot parse file {filePath} as the file does not exist.");
            }
        }

        private List<string> GrabLines(string filePath)
        {
            List<string> result = new List<string>();

            try
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        result.Add(s);
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}
