using ParseciniLibrary.Common.Templating;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Templating
{
    public class Template : ITemplate, IValidatorMethod
    {
        public Dictionary<string, TemplateElement> TemplateElements {  get; set; }

        public Template(TemplateElement rootTemplate)
        {
            TemplateElements.Add(rootTemplate.FilePath, rootTemplate);
        }
    }
}
