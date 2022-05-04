using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Exceptions
{
    public class FileParserException : Exception
    {
        public FileParserException(string message)
    : base(message)
        {
        }

        public FileParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
