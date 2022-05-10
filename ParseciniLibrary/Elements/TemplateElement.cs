using ParseciniLibrary.Common.Elements;
using ParseciniLibrary.Common.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Elements
{
    public class TemplateElement : ITemplateElement, IValidatorMethod
    {
        public string FilePath { get; set; }
        public string Content { get; set; }
    }
}
