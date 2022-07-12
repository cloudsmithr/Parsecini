using Dawn;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Elements;


namespace ParseciniLibrary.Validator
{
    // A class for validating a MarkdownElement.
    public class MarkdownElementValidator : IMarkdownElementValidator
    {
        private IMarkdownTagValidator markdownTagValidator;
        private IHTMLTagValidator htmlTagValidator;

        public MarkdownElementValidator(IMarkdownTagValidator _markdownTagValidator = null, IHTMLTagValidator _htmlTagValidator = null)
        {
            markdownTagValidator = _markdownTagValidator ?? new MarkdownTagValidator();
            htmlTagValidator = _htmlTagValidator ?? new HTMLTagValidator();
        }

        public bool Validate(MarkdownElement validationObject)
        {
            Guard.Argument(validationObject, nameof(validationObject)).NotNull();

            bool valid = true;

            // This is what happens when you don't know REGEX, kids.
            if (string.IsNullOrWhiteSpace(validationObject.markdownOpenSymbol))
            {
                valid = false;
                Log.LogFile("A MarkdownElement's markdownOpenSymbol cannot be empty or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(validationObject.htmlOpenSymbol))
            {
                valid = false;
                Log.LogFile("A MarkdownElement's htmlOpenSymbol cannot be empty or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(validationObject.htmlCloseSymbol) 
                && !(
                (validationObject.htmlOpenSymbol.Contains("{Content}") || validationObject.htmlOpenSymbol.Contains("{content}"))
                || validationObject.htmlOpenSymbol[validationObject.htmlOpenSymbol.Length - 2] == '/'))
            {
                valid = false;
                Log.LogFile("If no htmlCloseSymbol is specified the htmlOpenSymbol must either contain a self-closing forward slash '/' or the string '{content}'.");
            }
            if (validationObject.markdownOpenSymbol.Length == 1 && !string.IsNullOrWhiteSpace(validationObject.markdownCloseSymbol))
            {
                valid = false;
                Log.LogFile("A MarkdownElement's markdownOpenSymbol that is a character should have an empty markdownCloseSymbol, as this is not needed.");
            }
            if (validationObject.markdownOpenSymbol.Length > 1 && validationObject.markdownCloseSymbol.Length < 4
                || validationObject.markdownOpenSymbol.Length > 1 && validationObject.markdownOpenSymbol.Length < 3)
            {
                valid = false;
                Log.LogFile("A MarkdownElement's markdownOpenSymbol or markdownClosedSymbol that is a tag should begin with a closed bracket '[' and end with a closed bracket ']' and contain at least one character between them. A markdownClosedSymbol should also have a forward slash '/' immediately after the opening bracket '['.");
            }
            if (validationObject.markdownOpenSymbol.Length > 1 && validationObject.markdownCloseSymbol.Length < 4
                || validationObject.markdownOpenSymbol.Length > 1 && validationObject.markdownOpenSymbol.Length < 3
                && validationObject.markdownOpenSymbol != validationObject.markdownCloseSymbol.Remove(1, 1))
            {
                valid = false;
                Log.LogFile("A MarkdownElement that is a tag should have markdownOpenSymbol and markdownClosedSymbol that are the same, except for the markdownClosedSymbol's forward slash '/' directly after the opening bracket '['.");
            }

            if (!markdownTagValidator.Validate(new TagValidatorMethod(validationObject.markdownOpenSymbol)))
            {
                valid = false;
            }
            if (!markdownTagValidator.Validate(new TagValidatorMethod(validationObject.markdownCloseSymbol, true)))
            {
                valid = false;
            }
            if (!htmlTagValidator.Validate(new TagValidatorMethod(validationObject.htmlOpenSymbol)))
            {
                valid = false;
            }
            if (!htmlTagValidator.Validate(new TagValidatorMethod(validationObject.htmlCloseSymbol,true)))
            {
                valid = false;
            }

            return valid;
        }
    }
}
