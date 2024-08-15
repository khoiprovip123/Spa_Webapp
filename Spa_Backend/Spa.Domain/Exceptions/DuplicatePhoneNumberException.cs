using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Exceptions
{
    public class DuplicatePhoneNumberException : Exception
    {
        public DuplicatePhoneNumberException(string message) : base(message) { }
    }
}
