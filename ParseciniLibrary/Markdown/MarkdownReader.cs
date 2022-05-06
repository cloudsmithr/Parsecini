using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Markdown
{
    public class MarkdownReader
    {
        public MarkdownElement[] Elements;
        private Dictionary<string, MarkdownElement> MarkdownElementTags = new Dictionary<string, MarkdownElement>();
        private Dictionary<string, MarkdownElement> MarkdownElementSymbols = new Dictionary<string, MarkdownElement>();

        public IMarkdownElementValidator markdownElementValidator;
        public MarkdownReader(IConfigurationRoot myRoot, string configSection, IMarkdownElementValidator _markdownElementValidator)
        {
            markdownElementValidator = _markdownElementValidator;
            ValidateMarkdownSettings(myRoot, configSection);
        }

        public MarkdownElement[] ReadMarkdownElementsFromStringList(List<string> text)
        {
            bool isProcessingTag = false;

            return null;
        }

        private bool ValidateMarkdownSettings(IConfigurationRoot myRoot, string configSection)
        {
            Elements = myRoot.GetSection(configSection).Get<MarkdownElement[]>();

            foreach (MarkdownElement element in Elements)
            {
                if (markdownElementValidator.Validate(element))
                {
                    if (element.markdownOpenSymbol.StartsWith('['))
                        MarkdownElementTags.Add(element.markdownOpenSymbol, element);
                    else
                        MarkdownElementSymbols.Add(element.markdownOpenSymbol, element);
                }
                else
                {
                    throw new MarkdownFormatException($"Error in tag {element.name}. Please check the logs and adjust the appsettings Markdown configuration. The logs can be found at {Log.LogPath}");
                }
            }
            return true;
        }
    }
}
