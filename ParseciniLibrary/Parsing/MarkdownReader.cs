using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Common.Parsing;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Markdown;
using System.Collections.Generic;
using System.Linq;

namespace ParseciniLibrary.Parsing
{
    public class MarkdownReader : IObjectReader<MarkdownElement>
    {
        public IList<MarkdownElement> _objects { get; set; }
        public IMarkdownElementValidator markdownElementValidator;

        private Dictionary<string, MarkdownElement> MarkdownElementTags = new Dictionary<string, MarkdownElement>();
        private Dictionary<string, MarkdownElement> MarkdownElementSymbols = new Dictionary<string, MarkdownElement>();
        
        public MarkdownReader(IConfigurationRoot myRoot, string configSection, IMarkdownElementValidator _markdownElementValidator)
        {
            Log.LogFile("Building Markdownreader and validating Markdown config settings.");
            markdownElementValidator = _markdownElementValidator;
            ValidateMarkdownSettings(myRoot, configSection);
            Log.LogFile("Markdown config settings validated.");
        }

        public IList<MarkdownElement> ReadObjectsFromStringList(List<string> stringList)
        {
            bool isProcessingTag = false;


            return null;
        }

        private bool ValidateMarkdownSettings(IConfigurationRoot myRoot, string configSection)
        {
            _objects = myRoot.GetSection(configSection).Get<MarkdownElement[]>().ToList();

            foreach (MarkdownElement element in _objects)
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
