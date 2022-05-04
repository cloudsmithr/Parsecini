using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Exceptions
{
    public class ThemeBuilderException : Exception
    {
        public ThemeBuilderException(string message)
    : base(message)
        {
        }

        public ThemeBuilderException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
