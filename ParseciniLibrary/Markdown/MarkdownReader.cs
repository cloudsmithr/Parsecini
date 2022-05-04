using Microsoft.Extensions.Configuration;
using ParseciniLibrary.Exceptions;
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

        public MarkdownReader(IConfigurationRoot myRoot, string configSection)
        {
            Elements = myRoot.GetSection(configSection).Get<MarkdownElement[]>();

            foreach(MarkdownElement element in Elements)
            {
                string validationResult = ValidateMarkdownElement(element);
                if(validationResult == "true")
                {
                    if(element.markdownOpenSymbol.StartsWith('['))
                        MarkdownElementTags.Add(element.markdownOpenSymbol, element);
                    else
                        MarkdownElementSymbols.Add(element.markdownOpenSymbol, element);
                }
                else
                {
                    throw new MarkdownFormatException($"Error in tag {element.name}. Please check appsettings. The issue was: " + validationResult);
                }
            }

            Console.WriteLine("completed loading markdownreader");
        }

        public MarkdownElement[] ReadMarkdownElementsFromStringList(List<string> text)
        {
            bool isProcessingTag = false;

            return null;
        }

        private string ValidateMarkdownElement(MarkdownElement element)
        {
            if(string.IsNullOrWhiteSpace(element.markdownOpenSymbol))
            {
                return "A MarkdownElement's markdownOpenSymbol cannot be empty or whitespace.";
            }
            else if(string.IsNullOrWhiteSpace(element.htmlOpenSymbol))
            {
                return "A MarkdownElement's htmlOpenSymbol cannot be empty or whitespace.";
            }
            else if (string.IsNullOrWhiteSpace(element.htmlCloseSymbol) && !element.htmlOpenSymbol.Contains("{content}") && !element.htmlOpenSymbol.Contains("/"))
            {
                return "If no htmlCloseSymbol is specified the htmlOpenSymbol must either contain a self-closing forward slash '/' or the string '{content}'.";
            }
            
            if (element.markdownOpenSymbol.Length == 1 && !string.IsNullOrWhiteSpace(element.markdownCloseSymbol))
            {
                return "A MarkdownElement's markdownOpenSymbol that is a character should have an empty markdownCloseSymbol, as this is not needed.";
            }
            else if(element.markdownOpenSymbol.Length > 1 && element.markdownCloseSymbol.Length < 4
                || element.markdownOpenSymbol.Length > 1 && element.markdownOpenSymbol.Length < 3)
            {
                return "A MarkdownElement's markdownOpenSymbol or markdownClosedSymbol that is a tag should begin with a closed bracket '[' and end with a closed bracket ']' and contain at least one character between them. A markdownClosedSymbol should also have a forward slash '/' immediately after the opening bracket '['.";
            }
            else
            {
                if(element.markdownOpenSymbol.Length > 1 && element.markdownOpenSymbol != element.markdownCloseSymbol.Remove(1,1))
                {
                    return "A MarkdownElement that is a tag should have markdownOpenSymbol and markdownClosedSymbol that are the same, except for the markdownClosedSymbol's forward slash '/' directly after the opening bracket '['.";
                }

                string results1 = ValidateMarkdownSymbol(element.markdownOpenSymbol);
                string results2 = ValidateMarkdownSymbol(element.markdownCloseSymbol, true);
                string results3 = ValidateHTMLSymbol(element.htmlOpenSymbol);
                string results4 = ValidateHTMLSymbol(element.htmlCloseSymbol, true);

                if (results1 + results2 + results3 + results4 == "truetruetruetrue")
                {
                    return "true";
                }
                else
                {
                    return ((results1 == "true") ? "" : results1 + "; ")
                        + ((results2 == "true") ? "" : results2 + "; ")
                        + ((results3 == "true") ? "" : results3 + "; ")
                        + ((results4 == "true") ? "" : results4 + "; ");
                }
            }
        }

        private string ValidateHTMLSymbol(string tag, bool isClosingTag = false)
        {
            if (string.IsNullOrWhiteSpace(tag) && isClosingTag)
            {
                return "true";
            }
            if (tag[0] != '<' || tag[tag.Length-1] != '>' || tag.Length < 3)
                return "An HTML tag must begin with an opening angled bracket '<' and end with a closing angled bracket '>' and have at least one character between them.";
            if(tag.Length < 4 && isClosingTag)
                return "An closing HTML tag must being with an opening angled bracket and backslash '<\\' and end with a closing angled bracket '>' and have at least one character between them.";
            if (tag.Split('<').Length - 1 > 1 || tag.Split('>').Length - 1 > 1 || tag.Split('/').Length - 1 > 1)
                return "An HTML tag may not contain additional opening angled brackets '<' closing angled brackets '>' or forward slashes '/'.";

            return "true";
        }

        private string ValidateMarkdownSymbol(string tag, bool isClosingTag = false)
        {
            if (tag.Length > 1)
            {
                if (tag.StartsWith('[') )
                {
                    if (isClosingTag && tag[1] != '/')
                    {
                        return "A closing tag should have a forward slash '/' following the opening bracket '['";
                    }
                    if (!isClosingTag && tag[1] == '/')
                    {
                        return "An opening tag should not contain a forward slash '/' as the first character after the opening bracket '['. This is reserved for closing tags.";
                    }

                    if (tag.EndsWith(']'))
                    {
                        if (tag.Split('[').Length - 1 > 1 || tag.Split(']').Length - 1 > 1)
                        {
                            return "Do not use the opening brackets '[' or closing brackets ']' more than once in a tag.";
                        }
                        return "true";
                    }
                    else
                    {
                        return "A tag should end with a closing bracket.";
                    }
                }
                else 
                {
                    return "A MarkdownElement can be a tag or a character. A character must be exactly one character long, and a tag must begin with an opening bracket '[' and end with a closing bracket ']'.";
                }
            }
            else
            {
                if (tag == "[" || tag == "]")
                    return "A character cannot be '[' or ']'. These characters are reserved for tags.";
                else if (tag == "/")
                    return "A character cannot be a forward slash '/'. This character is reserved for closing tags.";
                else if (!isClosingTag && string.IsNullOrWhiteSpace(tag))
                    return "An opening tag cannot be whitespace. Also this line of code should never be reached.";
                else
                    return "true";
            }
        }
    }
}
