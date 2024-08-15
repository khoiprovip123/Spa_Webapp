using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Exceptions
{
    internal class EmptyListException : Exception
    {
        public EmptyListException() : base("List is empty")
        {
        }
    }
}
