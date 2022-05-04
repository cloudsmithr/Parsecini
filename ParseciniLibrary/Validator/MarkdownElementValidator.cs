using ParseciniLibrary.Common;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Validator
{
    public class MarkdownElementValidator : IValidator<MarkdownElement>
    {
        public bool Validate(MarkdownElement markdownElement)
        {
            bool valid = true;

            // This is what happens when you don't know REGEX, kids.
            if (string.IsNullOrWhiteSpace(markdownElement.markdownOpenSymbol))
            {
                valid = false;
                Log.LogFile("A MarkdownElement's markdownOpenSymbol cannot be empty or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(markdownElement.htmlOpenSymbol))
            {
                valid = false;
                Log.LogFile("A MarkdownElement's htmlOpenSymbol cannot be empty or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(markdownElement.htmlCloseSymbol) && !markdownElement.htmlOpenSymbol.Contains("{content}") && !markdownElement.htmlOpenSymbol.Contains("/"))
            {
                valid = false;
                Log.LogFile("If no htmlCloseSymbol is specified the htmlOpenSymbol must either contain a self-closing forward slash '/' or the string '{content}'.");
            }
            if (markdownElement.markdownOpenSymbol.Length == 1 && !string.IsNullOrWhiteSpace(markdownElement.markdownCloseSymbol))
            {
                valid = false;
                Log.LogFile("A MarkdownElement's markdownOpenSymbol that is a character should have an empty markdownCloseSymbol, as this is not needed.");
            }
            if (markdownElement.markdownOpenSymbol.Length > 1 && markdownElement.markdownCloseSymbol.Length < 4
                || markdownElement.markdownOpenSymbol.Length > 1 && markdownElement.markdownOpenSymbol.Length < 3)
            {
                valid = false;
                Log.LogFile("A MarkdownElement's markdownOpenSymbol or markdownClosedSymbol that is a tag should begin with a closed bracket '[' and end with a closed bracket ']' and contain at least one character between them. A markdownClosedSymbol should also have a forward slash '/' immediately after the opening bracket '['.");
            }
            if (markdownElement.markdownOpenSymbol.Length > 1 && markdownElement.markdownCloseSymbol.Length < 4
                || markdownElement.markdownOpenSymbol.Length > 1 && markdownElement.markdownOpenSymbol.Length < 3
                && markdownElement.markdownOpenSymbol != markdownElement.markdownCloseSymbol.Remove(1, 1))
            {
                valid = false;
                Log.LogFile("A MarkdownElement that is a tag should have markdownOpenSymbol and markdownClosedSymbol that are the same, except for the markdownClosedSymbol's forward slash '/' directly after the opening bracket '['.");
            }

            MarkdownTagValidator markdownTagValidator = new MarkdownTagValidator();
            HTMLTagValidator htmlTagValidator = new HTMLTagValidator();

            if (!markdownTagValidator.Validate(new TagValidatorMethod(markdownElement.markdownOpenSymbol)))
            {
                valid = false;
            }
            if (!markdownTagValidator.Validate(new TagValidatorMethod(markdownElement.markdownCloseSymbol, true)))
            {
                valid = false;
            }
            if (!htmlTagValidator.Validate(new TagValidatorMethod(markdownElement.htmlOpenSymbol)))
            {
                valid = false;
            }
            if (!htmlTagValidator.Validate(new TagValidatorMethod(markdownElement.htmlCloseSymbol,true)))
            {
                valid = false;
            }

            return valid;
        }
    }
}
