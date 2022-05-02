using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Common
{
    public interface ITextParser : IParser
    {
        public string fileExtension { get; set; }
        public List<string> ParseFile(string filePath);
    }
}
