using ParseciniLibrary.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Unit.Tests.Parsing
{
    public static class MarkdownReaderTestsData
    {
        public static IEnumerable<object[]> MarkdownReaderReadObjectsSuccess
        {
            get
            {
                return new[]
                {
                    new object[] {
                        "checking simple text",
                        new List<string>
                        {
                            "Sup",
                            "testing what this thing does with simple text"
                        },
                        new List<MarkdownElement>
                        {
                        }
                    },

                    new object[] {
                        "checking simple header",
                        new List<string>
                        {
                            "# Sup",
                        },
                        new List<MarkdownElement>
                        {
                            new MarkdownElement
                            {
                                name = "Header1",
                                markdownOpenSymbol = "#",
                                markdownCloseSymbol = "",
                                htmlOpenSymbol = "<h1>",
                                htmlCloseSymbol = "</h1>",
                                Content = "Sup"
                            }
                        }
                    },
                    new object[] {
                        "checking empty simple header",
                        new List<string>
                        {
                            "#",
                        },
                        new List<MarkdownElement>
                        {
                            new MarkdownElement
                            {
                                name = "Header1",
                                markdownOpenSymbol = "#",
                                markdownCloseSymbol = "",
                                htmlOpenSymbol = "<h1>",
                                htmlCloseSymbol = "</h1>",
                                Content = ""
                            }
                        }
                    },

                    new object[] {
                        "checking simple text tag block",
                        new List<string>
                        {
                            "[text]",
                            "The end had to come.",
                            "It's the truth you've always known.",
                            "Time.",
                            "[/text]"
                        },
                        new List<MarkdownElement>
                        {
                            new MarkdownElement
                            {
                                name = "Paragraph1",
                                markdownOpenSymbol = "[text]",
                                markdownCloseSymbol = "[/text]",
                                htmlOpenSymbol = "<p>",
                                htmlCloseSymbol = "</p>",
                                Content = "The end had to come.It's the truth you've always known.Time."
                            }
                        }
                    },

                    new object[] {
                        "checking empty simple text tag block",
                        new List<string>
                        {
                            "[text]",
                            "[/text]"
                        },
                        new List<MarkdownElement>
                        {
                            new MarkdownElement
                            {
                                name = "Paragraph1",
                                markdownOpenSymbol = "[text]",
                                markdownCloseSymbol = "[/text]",
                                htmlOpenSymbol = "<p>",
                                htmlCloseSymbol = "</p>",
                                Content = ""
                            }
                        }
                    },

                    new object[] {
                        "checking header and text tag block together",
                        new List<string>
                        {
                            "# Hello",
                            "[text]",
                            "The end had to come.",
                            "It's the truth you've always known.",
                            "Time.",
                            "[/text]"
                        },
                        new List<MarkdownElement>
                        {
                            new MarkdownElement
                            {
                                name = "Header1",
                                markdownOpenSymbol = "#",
                                markdownCloseSymbol = "",
                                htmlOpenSymbol = "<h1>",
                                htmlCloseSymbol = "</h1>",
                                Content = "Hello"
                            },
                            new MarkdownElement
                            {
                                name = "Paragraph1",
                                markdownOpenSymbol = "[text]",
                                markdownCloseSymbol = "[/text]",
                                htmlOpenSymbol = "<p>",
                                htmlCloseSymbol = "</p>",
                                Content = "The end had to come.It's the truth you've always known.Time."
                            }
                        }
                    },

                    new object[] {
                        "checking header and text tag block and image together",
                        new List<string>
                        {
                            "# Hello",
                            "[text]",
                            "The end had to come.",
                            "It's the truth you've always known.",
                            "Time.",
                            "[/text]",
                            "+ img/test.jpg"
                        },
                        new List<MarkdownElement>
                        {
                            new MarkdownElement
                            {
                                name = "Header1",
                                markdownOpenSymbol = "#",
                                markdownCloseSymbol = "",
                                htmlOpenSymbol = "<h1>",
                                htmlCloseSymbol = "</h1>",
                                Content = "Hello"
                            },
                            new MarkdownElement
                            {
                                name = "Paragraph1",
                                markdownOpenSymbol = "[text]",
                                markdownCloseSymbol = "[/text]",
                                htmlOpenSymbol = "<p>",
                                htmlCloseSymbol = "</p>",
                                Content = "The end had to come.It's the truth you've always known.Time."
                            },
                            new MarkdownElement
                            {
                                name = "Image1",
                                markdownOpenSymbol = "+",
                                markdownCloseSymbol = "",
                                htmlOpenSymbol = "<img {content}>",
                                htmlCloseSymbol = "",
                                Content = "img/test.jpg"
                            }
                        }
                    }

                };
            }
        }

        public static IEnumerable<object[]> MarkdownReaderReadObjectsFailure
        {
            get
            {
                return new[]
                {
                     new object[] {
                        "checking fake tag",
                        new List<string>
                        {
                            "[faketag]",
                            "[/faketag]"
                        },
                        "Unknown tag '[faketag]' in line 1. Please check the markdown file and the MarkdownElements section of the appsettings."
                    },
                    
                    new object[] {
                        "checking unexpected closing tag",
                        new List<string>
                        {
                            "[/text]",
                            "# Sup",
                        },
                        "Unexpected closing tag '[/text]' in line 1. Please check the markdown file and ensure every closing tag has an opening tag."
                    },

                    new object[] {
                        "checking fake closing tag",
                        new List<string>
                        {
                            "[text]",
                            "[/toxt]",
                        },
                        "Unknown tag '[/toxt]' in line 2. Please check the markdown file and the MarkdownElements section of the appsettings."
                    },

                    new object[] {
                        "checking fake closing tag",
                        new List<string>
                        {
                            "[text]",
                            "[text]",
                        },
                        "Unexpected opening tag '[text]' in line 2. Please check the markdown file and ensure every opening tag has a corresponding closing tag."
                    },

                    new object[] {
                        "checking header inside of text block",
                        new List<string>
                        {
                            "[text]",
                            "# Sup",
                            "[/text]"
                        },
                        "Unexpected symbol '#' in line 2. You cannot use markdown symbols within a markdown tag."
                    },

                };
            }
        }
    }
}
