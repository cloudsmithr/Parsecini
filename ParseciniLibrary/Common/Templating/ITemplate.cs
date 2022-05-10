using ParseciniLibrary.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Common.Templating
{
    public interface ITemplate : ITemplatable
    {
        public Dictionary<string, TemplateElement> TemplateElements { get; set; }

    }
}
