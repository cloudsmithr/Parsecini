using System.Collections.Generic;

namespace ParseciniLibrary.Common.Parsing
{
    public interface ITextParser : IParser
    {
        public string fileExtension { get; set; }
        public List<string> ParseFile(string filePath);
        public string ParseFile(string filePath, string lineJoinString);
        public string JoinString(string separator, IList<string> stringList);
    }
}
