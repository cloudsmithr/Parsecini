using ParseciniLibrary.Common;
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
            fileExtension = _fileExtension;
        }

        public List<string> ParseFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (Path.GetExtension(filePath) == fileExtension)
                {
                    // It's a File, and it exists, and has the right fileExtension we can read it.
                    return GrabLines(filePath);
                }
            }

            return null;
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
