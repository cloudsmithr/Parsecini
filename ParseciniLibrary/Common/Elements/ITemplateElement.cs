using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Common.Elements
{
    public interface ITemplateElement : IElement
    {
        public string FilePath { get; set; }
        public string Content {  get; set; }
    }
}
