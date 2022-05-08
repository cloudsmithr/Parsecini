using Dawn;
using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Common.Parsing;
using ParseciniLibrary.Common.Validator;
using ParseciniLibrary.Exceptions;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParseciniLibrary.Parsing
{
    // A class for constructing a list of MarkdownElements out of a list of strings.
    public class MarkdownReader : IObjectReader<MarkdownElement>
    {
        public IList<MarkdownElement> _objects { get; set; }
        public IMarkdownElementValidator markdownElementValidator;

        private Dictionary<string, MarkdownElement> MarkdownElementTags = new Dictionary<string, MarkdownElement>();
        private Dictionary<string, MarkdownElement> MarkdownElementSymbols = new Dictionary<string, MarkdownElement>();
        
        public MarkdownReader(IConfigurationRoot myRoot, string configSection, IMarkdownElementValidator _markdownElementValidator)
        {
            Guard.Argument(myRoot, nameof(myRoot)).NotNull();
            Guard.Argument(configSection, nameof(configSection)).NotWhiteSpace().NotNull();
            Guard.Argument(_markdownElementValidator, nameof(_markdownElementValidator)).NotNull();

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
            int lineCounter = 1;

            foreach (string s in stringList)
            {
                if (!string.IsNullOrEmpty(s))
                {

                    if (s[0] == '[')
                    {
                        string tag = MarkdownTagFromString(s);
                        // Dealing with a tag, let's strip out the tag and throw away any excess content, maybe log that anything after the closing bracket won't be output.
                        if (!isProcessingTag)
                        {
                            // We are not currently processing a tag. So we only expect opening tags. Throw if there's a closing tag (use lineCounter variable)
                            if (!IsClosingTag(tag))
                            {
                                // We have a new opening tag! Create a new currentMarkdownElement and assign it the proper one from the dictionary
                                if (MarkdownElementTags.ContainsKey(tag))
                                {
                                    currentMarkdownElement = MarkdownElementTags[tag].Clone();
                                    isProcessingTag = true;
                                    contentStringBuilder.Clear();
                                }
                                else
                                {
                                    // Throw exception that the tag read in was not part of the tags set in the appsettings.
                                    throw new MarkdownReaderException($"Unknown tag '{tag}' in line {lineCounter}. Please check the markdown file and the MarkdownElements section of the appsettings.");
                                }
                            }
                            else
                            {
                                //throw exception
                                throw new MarkdownReaderException($"Unexpected closing tag '{tag}' in line {lineCounter}. Please check the markdown file and ensure every closing tag has an opening tag.");
                            }
                        }
                        else
                        {
                            // We are currently processing a tag. So we only expect closing tags. Throw if there's an opening tag (use lineCounter variable)
                            if (IsClosingTag(tag))
                            {
                                // We have a new opening tag! Create a new currentMarkdownElement and assign it the proper one from the dictionary
                                if (MarkdownElementTags.ContainsKey(ClosingMarkdownTagToOpen(tag)))
                                {
                                    currentMarkdownElement.Content = contentStringBuilder.ToString();
                                    results.Add(currentMarkdownElement.Clone());
                                    isProcessingTag = false;
                                }
                                else
                                {
                                    // Throw exception that the tag read in was not part of the tags set in the appsettings.
                                    throw new MarkdownReaderException($"Unknown tag '{tag}' in line {lineCounter}. Please check the markdown file and the MarkdownElements section of the appsettings.");
                                }
                            }
                            else
                            {
                                //throw exception
                                throw new MarkdownReaderException($"Unexpected opening tag '{tag}' in line {lineCounter}. Please check the markdown file and ensure every opening tag has a corresponding closing tag.");
                            }
                        }
                    }
                    else
                    {
                        string symbol = MarkdownSymbolFromString(s);
                        if (MarkdownElementSymbols.ContainsKey(symbol.ToString()))
                        {
                            //dealing with a symbol, let's strip out the tag and everything that comes after that to the content. Also strip both beginning and ending whitespace.
                            if (!isProcessingTag)
                            {
                                currentMarkdownElement = MarkdownElementSymbols[symbol].Clone();
                                currentMarkdownElement.Content = MarkdownContentFromSymbolString(s);
                                results.Add(currentMarkdownElement.Clone());
                            }
                            else
                            {
                                // using a Symbol inside of a Tag is invalid. Throw exception here to let user know what line it happened on (use lineCounter variable)
                                throw new MarkdownReaderException($"Unexpected symbol '{symbol}' in line {lineCounter}. You cannot use markdown symbols within a markdown tag.");
                            }
                        }
                        else
                        {
                            // We are dealing with a text line.
                            if (isProcessingTag)
                            {
                                // If we are processing a tag we should add the string to the content
                                contentStringBuilder.Append(s);
                            }
                            else
                            {
                                // If we are not currently processing a tag, we can ignore all text lines. Maybe write to the log that we're skipping the line if there's text there. 
                                Log.LogFile($"Text outside of a tag block and without a markdown symbol in line {lineCounter} will be ignored.");
                            }
                        }
                    }
                }

                lineCounter++;
            }

            return results;
        }

        private bool ValidateMarkdownSettings(IConfigurationRoot myRoot, string configSection)
        {
            try
            {
                _objects = myRoot.GetSection(configSection).Get<MarkdownElement[]>().ToList();
            }
            catch(Exception e)
            {
                throw new MarkdownFormatException($"Could not read the appsettings Markdown configuration from section {configSection}.", e);
            }

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

        private string MarkdownTagFromString(string str)
        {
            return (str.Split(']'))[0] + "]";
        }

        private string MarkdownContentFromSymbolString(string str)
        {
            return str.Remove(0, 1).Trim();
        }

        private string MarkdownSymbolFromString(string str)
        {
            return str[0].ToString();
        }

        private string ClosingMarkdownTagToOpen(string str)
        {
            return str.Remove(1, 1);
        }

        private bool IsClosingTag(string str)
        {
            return str[1] == '/';
        }

    }
}
