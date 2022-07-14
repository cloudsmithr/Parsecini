using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Templating
{
    public class BlogNavigation
    {
        public string FirstPageNav { get; set; }
        public string PrevPageNav { get; set; }
        public string NextPageNav { get; set; }
        public string LastPageNav { get; set; }

        public string WriteMe(string firstPageUrl = "", string prevPageUrl = "", string nextPageUrl = "", string lastPageUrl = "")
        {
            string result = "";

            if (firstPageUrl != "")
                result += FirstPageNav.Replace("{url}", firstPageUrl);
            else
                result += FirstPageNav.Replace("{url}", "#");

            if (prevPageUrl != "")
                result += PrevPageNav.Replace("{url}", prevPageUrl);
            else
                result += PrevPageNav.Replace("{url}", "#");
            
            if (nextPageUrl != "")
                result += NextPageNav.Replace("{url}", nextPageUrl);
            else
                result += NextPageNav.Replace("{url}", "#");
            
            if (lastPageUrl != "")
                result += LastPageNav.Replace("{url}", lastPageUrl);
            else
                result += LastPageNav.Replace("{url}", "#");

            return result;
        }
    }
}
