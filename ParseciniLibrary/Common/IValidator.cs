using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Common
{
    public interface IValidator<T>
    {
        public bool Validate(T obj);
    }
}
