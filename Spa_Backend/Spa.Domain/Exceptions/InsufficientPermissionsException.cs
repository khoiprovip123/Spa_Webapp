using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Exceptions
{
    public class InsufficientPermissionsException : Exception
    {
        public InsufficientPermissionsException(string message) : base(message)
        {
        }
    }
}
