using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Unit.Tests.Validator
{
    public class TemplateValidatorTestsData
    {
        public static IEnumerable<object[]> TemplateValidatorValidationSuccess
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
    }
}
