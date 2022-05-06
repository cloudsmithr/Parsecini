using Dawn;
using ParseciniLibrary.Common;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Logging;
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
            Guard.Argument(markdownValidatorMethod, nameof(markdownValidatorMethod)).NotNull();

            string tag = markdownValidatorMethod.tag;
            bool isClosingTag = markdownValidatorMethod.isClosingTag;

            bool valid = true;

            if (tag.Length > 1)
            {
                if (!tag.StartsWith('['))
                {
                    valid = false;
                    Log.LogFile("A MarkdownElement can be a tag or a character. A character must be exactly one character long, and a tag must begin with an opening bracket '[' and end with a closing bracket ']'.");
                }
                if (!tag.EndsWith(']'))
                {
                    valid = false;
                    Log.LogFile("A tag should end with a closing bracket.");
                }
                if (isClosingTag && tag[1] != '/')
                {
                    valid = false;
                    Log.LogFile("A closing tag should have a forward slash '/' following the opening bracket '['");
                }
                if (!isClosingTag && tag[1] == '/')
                {
                    valid = false;
                    Log.LogFile("An opening tag should not contain a forward slash '/' as the first character after the opening bracket '['. This is reserved for closing tags.");
                }

                if (tag.Split('[').Length - 1 > 1 || tag.Split(']').Length - 1 > 1)
                {
                    valid = false;
                    Log.LogFile("Do not use the opening brackets '[' or closing brackets ']' more than once in a tag.");
                }
                if (tag.Split('/').Length - 1 > 1)
                {
                    valid = false;
                    Log.LogFile("Do not use forward slashes '/' more than once in a tag.");
                }
                if(tag.Length <= 2 && !isClosingTag)
                {
                    valid = false;
                    Log.LogFile("A tag must have an opening bracket '[' and a closing bracket ']' with at least one valid character between them.");
                }
                if (tag.Length <= 3 && isClosingTag)
                {
                    valid = false;
                    Log.LogFile("A closing tag must have an opening bracket '[' followed by a forward slash '/', followed by at least one valid character, then and a closing bracketa ']'.");
                }
            }
            else
            {
                if (tag == "[" || tag == "]")
                {
                    valid = false;
                    Log.LogFile("A character cannot be '[' or ']'. These characters are reserved for tags.");
                }
                if (tag == "/")
                {
                    valid = false;
                    Log.LogFile("A character cannot be a forward slash '/'. This character is reserved for closing tags.");
                }
                if (!isClosingTag && string.IsNullOrWhiteSpace(tag))
                {
                    valid = false;
                    Log.LogFile("An opening tag cannot be whitespace. Also this line of code should never be reached.");
                }
            }

            return valid;
        }
    }
}
