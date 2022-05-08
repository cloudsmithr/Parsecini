using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Exceptions
{
    public class MarkdownReaderException : Exception
    {
        public MarkdownReaderException(string message)
            : base(message)
        {
        }

        public MarkdownReaderException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
