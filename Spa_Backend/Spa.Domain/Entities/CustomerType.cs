using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class CustomerType
    {
        public int CustomerTypeID { get; set; }
        public string CustomerTypeName { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
