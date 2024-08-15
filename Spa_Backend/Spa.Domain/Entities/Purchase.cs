using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Purchase
    {
        public long SaleID { get; set; }
        public long PurchaseID { get; set; }
      
        public long ProductID { get; set; }
       
        public int Quantity { get; set; }

        public Product Product { get; set; }

        public Sale Sale { get; set; }
    }
}
