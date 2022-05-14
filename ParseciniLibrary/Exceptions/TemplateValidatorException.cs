using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Exceptions
{
    public class TemplateValidatorException : Exception
    {
        public TemplateValidatorException(string message)
            : base(message)
        {
        }

        public TemplateValidatorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
