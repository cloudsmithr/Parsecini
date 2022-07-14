using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Templating
{
    public class BlogIndex
    {
        public List<Post> Posts = new List<Post>();
        public int PostsPerPage { get; set; }
        public string TemplatePath { get; set; }
        public string PreviewTemplatePath { get; set; }
        public BlogNavigation BlogNavigation { get; set; }
        public string Url { get; set; }

        public string IndexUrl(int index)
        {
            return Url.Replace(".html", $"{index}.html");
        }
    }
}
