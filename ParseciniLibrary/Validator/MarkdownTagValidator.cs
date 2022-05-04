using ParseciniLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Validator
{
    public class MarkdownTagValidator : IValidator<TagValidatorMethod>
    {
        public bool Validate(TagValidatorMethod markdownValidatorMethod)
        {
            return true;
        }
    }
}
