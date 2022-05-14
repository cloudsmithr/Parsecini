using ParseciniLibrary.Common.Templating;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Templating
{
    public class Template : ITemplate, IValidatorMethod
    {
        public Dictionary<string, TemplateElement> TemplateElements {  get; set; }

        public string rootTemplateName;
        public string rootTemplatePath;

        public Template(TemplateElement rootTemplate, string rootFilePath)
        {
            TemplateElements = new Dictionary<string, TemplateElement>();
            TemplateElements.Add(rootTemplate.FilePath, rootTemplate);
            rootTemplateName = new FileInfo(rootTemplate.FilePath).Name;
            rootTemplatePath = new FileInfo(rootFilePath).DirectoryName;
        }

        public string WriteMe()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(TemplateElements[rootTemplateName].Content);

            string originalString = "";  stringBuilder.ToString();

            while(originalString != stringBuilder.ToString())
            {
                originalString = stringBuilder.ToString();

                foreach (KeyValuePair<string, TemplateElement> kvp in TemplateElements)
                {
                    stringBuilder.Replace($"(tpl)[{kvp.Value.FilePath}]", kvp.Value.Content);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
