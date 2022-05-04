using ParseciniLibrary.Common;
using ParseciniLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Validator
{
    public class HTMLTagValidator : IValidator<TagValidatorMethod>
    {
        public bool Validate(TagValidatorMethod tagValidatorMethod)
        {
            string tag = tagValidatorMethod.tag;
            bool isClosingTag = tagValidatorMethod.isClosingTag;

            bool valid = true;

            if (tag[0] != '<' || tag[tag.Length - 1] != '>' || tag.Length < 3)
            {
                valid = false;
                Log.LogFile("An HTML tag must begin with an opening angled bracket '<' and end with a closing angled bracket '>' and have at least one character between them.");
            }
            if (tag.Length < 4 && isClosingTag)
            {
                valid = false;
                Log.LogFile("A closing HTML tag must being with an opening angled bracket and backslash '<\\' and end with a closing angled bracket '>' and have at least one character between them.");
            }
            if (tag.Split('<').Length - 1 > 1 || tag.Split('>').Length - 1 > 1 || tag.Split('/').Length - 1 > 1)
            {
                valid = false;
                Log.LogFile("An HTML tag may not contain additional opening angled brackets '<' closing angled brackets '>' or forward slashes '/'.");
            }

            return valid;
        }
    }
}
