using ParseciniLibrary.Common;
using ParseciniLibrary.Common.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Validator
{
    public class MarkdownTagValidator : IMarkdownTagValidator
    {
        public bool Validate(TagValidatorMethod markdownValidatorMethod)
        {
            return true;
        }
    }
}
