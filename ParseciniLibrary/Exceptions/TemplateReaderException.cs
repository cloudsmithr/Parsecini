using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Exceptions
{
    internal class TemplateReaderException : Exception
    {
        public TemplateReaderException(string message)
    : base(message)
        {
        }

        public TemplateReaderException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
