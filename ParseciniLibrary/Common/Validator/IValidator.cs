using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary.Common.Validator
{
    public interface IValidator<T> where T : IValidatorMethod
    {
        public bool Validate(T validationObject);
    }
}
