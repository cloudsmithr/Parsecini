using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Common.Parsing;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Markdown;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            IList<MarkdownElement> results = new List<MarkdownElement>();

            MarkdownElement currentMarkdownElement = new MarkdownElement();
            StringBuilder contentStringBuilder = new StringBuilder();
            int lineCounter = 0;

            foreach(string s in stringList)
            {
                if(s[0] == '[')
                {
                    // Dealing with a tag, let's strip out the tag and throw away any excess content, maybe log that anything after the closing bracket won't be output.
                    if (!isProcessingTag)
                    {
                        // We are not currently processing a tag. So we only expect opening tags. Throw if there's a closing tag (use lineCounter variable)
                        isProcessingTag = true;
                    }
                    else
                    {
                        // We are currently processing a tag. So we only expect closing tags. Throw if there's an opening tag (use lineCounter variable)
                        isProcessingTag = false;
                    }

                }
                else if(MarkdownElementSymbols.ContainsKey(s))
                {
                    //dealing with a symbol, let's strip out the tag and everything that comes after that to the content. Also strip both beginning and ending whitespace.
                    if(!isProcessingTag)
                    {
                        // We are not currently processing a tag, so we can proceed as normal
                    }
                    else
                    {
                        // using a Symbol inside of a Tag is invalid. Throw exception here to let user know what line it happened on (use lineCounter variable)
                    }
                }
                else
                {
                    // We are dealing with a text line.
                    if(isProcessingTag)
                    {
                        // If we are processing a tag we should add the string to the content
                    }
                    else
                    {
                        // If we are not currently processing a tag, we can ignore all text lines. Maybe write to the log that we're skipping the line if there's text there. 
                    }
                }

                lineCounter++;
            }

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
