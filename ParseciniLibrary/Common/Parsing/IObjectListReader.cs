using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Common.Parsing
{
    public interface IObjectListReader<T>
    {
        public IList<T> _objects { get; set; }
        public IList<T> ReadObjectsFromStringList(List<string> stringList);
    }
}
