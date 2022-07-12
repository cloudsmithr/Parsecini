using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Templating
{
    public class Website
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public List<Page> Pages { get; set; }
    }
}
