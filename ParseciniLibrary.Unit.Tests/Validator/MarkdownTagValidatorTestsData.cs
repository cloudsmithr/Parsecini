using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Unit.Tests.Validator
{
    public class MarkdownTagValidatorTestsData
    {
        public static IEnumerable<object[]> MarkdownTagValidatorValidationSuccess
        {
            get
            {
                return new[]
                {
                    new object[] { "#" },
                    new object[] { "+", true },
                    new object[] { "<" },
                    new object[] { ">", true },
                    new object[] { "[a]" },
                    new object[] { "[/a]", true },
                    new object[] { "[hello]" },
                    new object[] { "[/hello]", true },
                };
            }
        }

        public static IEnumerable<object[]> MarkdownTagValidatorValidationFailure
        {
            get
            {
                return new[]
                {
                    new object[] { "" },
                    new object[] { "[" },
                    new object[] { "[", true },
                    new object[] { "]" },
                    new object[] { "]", true },
                    new object[] { "/" },
                    new object[] { "/", true },
                    new object[] { "[/" },
                    new object[] { "[/", true },
                    new object[] { "[/]" },
                    new object[] { "[/]", true },
                    new object[] { "[]" },
                    new object[] { "[]", true },
                    new object[] { "[hel[aa]"},
                    new object[] { "[hel[aa]", true },
                    new object[] { "[hel]aa]" },
                    new object[] { "[hel]aa]", true },
                    new object[] { "[/hel/aa]" },
                    new object[] { "[/hel/aa]", true },
                    new object[] { "[hello]", true },
                };
            }
        }
    }
}
