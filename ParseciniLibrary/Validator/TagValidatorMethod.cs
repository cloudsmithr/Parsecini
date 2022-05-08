using ParseciniLibrary.Common;
using ParseciniLibrary.Common.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Validator
{
    // A class for storing information needed to validate a Tag (ie Markdown or HTML tag).
    public class TagValidatorMethod : IValidatorMethod
    {
        public string tag { get; set; }
        public bool isClosingTag { get; set; }

        public TagValidatorMethod(string _tag, bool _isClosingTag = false)
        {
            tag = _tag;
            isClosingTag = _isClosingTag;
        }
    }
}
