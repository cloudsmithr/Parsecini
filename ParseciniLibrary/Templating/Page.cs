using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Templating
{
    public class Page
    {
        public string Title { get; set; }
        public string Url { get; set;  }
        public string Template { get; set; }
        public string Markdown { get; set; }
        public string Posts { get; set; }
        public int Pagination { get; set; }
        public bool IsBlogPage { get; set; }


        public Page(string title, string url, string template, string markdown = "", string posts = "", int pagination = 10)
        {
            Title = title;
            Url = url;
            Template = template;
            IsBlogPage = false;

            if (!string.IsNullOrWhiteSpace(posts))
            {
                IsBlogPage = true;
                Posts = posts;
                Pagination = pagination;
            }
            else
            {
                Markdown = markdown;
            }
        }
    }
}
