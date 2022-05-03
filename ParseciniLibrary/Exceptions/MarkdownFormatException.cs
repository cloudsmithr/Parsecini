using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Exceptions
{
    public class MarkdownFormatException : Exception
    {
        public MarkdownFormatException(string message)
            : base(message)
        {
        }

        public MarkdownFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
