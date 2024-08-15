using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Sale
    {
        public long SaleID { get; set; }
        public DateTime SaleDate { get; set; }
        public long CustomerID { get; set; } 
        public long EmployeeID { get; set; }
     
        public double Total { get; set; }

       // relationship
        public Employee Employee { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}
