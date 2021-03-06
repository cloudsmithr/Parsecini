using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Common.Parsing
{
    public interface IObjectReader<T>
    {
        public T _object { get; set; }
        public T ReadObjectFromString(string str);
    }
}
