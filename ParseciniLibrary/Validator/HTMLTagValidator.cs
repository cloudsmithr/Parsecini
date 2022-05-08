using Dawn;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Logging;

namespace ParseciniLibrary.Validator
{
    // A class for validating an HTML tag.
    public class HTMLTagValidator : IHTMLTagValidator
    {
        public bool Validate(TagValidatorMethod validationObject)
        {
            Guard.Argument(validationObject, nameof(validationObject)).NotNull();

            string tag = validationObject.tag;
            bool isClosingTag = validationObject.isClosingTag;

            bool valid = true;

            if(!string.IsNullOrWhiteSpace(tag))
            {
                if (tag[0] != '<' || tag[tag.Length - 1] != '>' || tag.Length < 3)
                {
                    valid = false;
                    Log.LogFile("An HTML tag must begin with an opening angled bracket '<' and end with a closing angled bracket '>' and have at least one character between them.");
                }
                if (tag.Length < 4 && isClosingTag)
                {
                    valid = false;
                    Log.LogFile("A closing HTML tag must being with an opening angled bracket and forward slash '</' and end with a closing angled bracket '>' and have at least one character between them.");
                }
                if (tag.Split('<').Length - 1 > 1 || tag.Split('>').Length - 1 > 1 || tag.Split('/').Length - 1 > 1)
                {
                    valid = false;
                    Log.LogFile("An HTML tag may not contain additional opening angled brackets '<' closing angled brackets '>' or forward slashes '/'.");
                }
                if (!isClosingTag && tag.Contains("/") && tag[tag.Length - 2] != '/')
                {
                    valid = false;
                    Log.LogFile("An opening HTML tag may only contain a forward slash '/' immediately before the closing angled bracket '>' to indicate a self-closing opening tag.");
                }
            }

            return valid;
        }
    }
}
