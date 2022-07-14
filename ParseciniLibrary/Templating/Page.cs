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
        public string RootUrl { get; set;  }
        public string Template { get; set; }
        public string Markdown { get; set; }
        public string Posts { get; set; }
        public int PostsPerPage { get; set; }
        public int MaxPageLinksPerPage { get; set; }
        public bool IsBlogPage { get; set; }
        public string PreviewTemplate { get; set; }
        public string BlogNavigationTemplate { get; set; }

        public Page(string title, string rooturl, string template, string markdown = "", string posts = "", int postsperpage = 10, string previewTemplate = "", string blognavigationtemplate = "", int maxpagelinksperpage = 10)
        {
            Title = title;
            RootUrl = rooturl;
            Template = template;
            IsBlogPage = false;

            if (!string.IsNullOrWhiteSpace(posts))
            {
                IsBlogPage = true;
                Posts = posts;
                PostsPerPage = postsperpage;
                MaxPageLinksPerPage = maxpagelinksperpage;
                PreviewTemplate = previewTemplate;
                BlogNavigationTemplate = blognavigationtemplate;
            }
            else
            {
                Markdown = markdown;
            }
            PreviewTemplate = previewTemplate;
        }
    }
}
