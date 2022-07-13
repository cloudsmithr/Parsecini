using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Templating
{
    public class Post
    {
        public Dictionary<string, string> Variables { get; set; }
        public DateTime PostDate { get; }
        public string Url { get; }

        public Post(DateTime postDate, string url)
        {
            PostDate = postDate;
            Url = url;
        }
    }
}
