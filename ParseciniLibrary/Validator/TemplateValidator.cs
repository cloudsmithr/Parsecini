using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Elements;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Validator
{
    public class TemplateValidator : ITemplateValidator
    {
        private Dictionary<string, TemplateElement> templateElements;

        public bool Validate(Template validationObject)
        {
            templateElements = validationObject.TemplateElements;

            ValidateTemplateElement(templateElements[validationObject.rootTemplateName], templateElements[validationObject.rootTemplateName].FilePath);
            return true;
        }

        private void ValidateTemplateElement(TemplateElement templateElement, string templateBranchString)
        {
            if(!string.IsNullOrWhiteSpace(templateElement.Content))
            {
                foreach (KeyValuePair<string, TemplateElement> entry in templateElements)
                {
                    if (templateElement.Content.Contains(templateElement.FilePath))
                    {
                        throw new TemplateValidatorException($"Circular reference detected in template file {templateElement.FilePath}. Template references itself.");
                    }
                    else if(templateElement.Content.Contains(entry.Value.FilePath))
                    {
                        // Make sure that we haven't been to this template before
                        if (templateBranchString.Contains(entry.Value.FilePath))
                        {
                            throw new TemplateValidatorException($"Circular reference detected in file {templateElement.FilePath}. Reference {entry.Value.FilePath} already exists in template branch {templateBranchString}");
                        }

                        ValidateTemplateElement(entry.Value, templateBranchString + "->" + entry.Value.FilePath);
                    }
                    else
                    {
                        Log.LogFile($"Successfully validate template branch: {templateBranchString}");
                    }
                }
            }
        }
    }
}
