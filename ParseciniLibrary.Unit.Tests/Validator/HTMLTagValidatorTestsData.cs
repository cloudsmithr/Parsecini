using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Unit.Tests.Validator
{
    public class HTMLTagValidatorTestsData
    {

        public static IEnumerable<object[]> HTMLTagValidatorValidationSuccess
        {
            get
            {
                return new[]
                {
                    new object[] { "<h1>" },
                    new object[] { "</h1>", true },
                    new object[] { "<input {content} />" },
                    new object[] { "<input />" },
                };
            }
        }

        public static IEnumerable<object[]> HTMLTagValidatorValidationFailure
        {
            get
            {
                return new[]
                {
                    new object[] { "<>" },
                    new object[] { "</>", true },
                    new object[] { "<h1" },
                    new object[] { "</h1" },
                    new object[] { "</" },
                    new object[] { "<" },
                    new object[] { "</h1/>" },
                };
            }
        }
    }
}
