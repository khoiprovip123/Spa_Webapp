using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Warehouse
    {
        public long BranchID { get; set; }
      
        public long ProductID { get; set; }
    
        public DateTime ImportDate { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; } 
        public Branch Branch { get; set; }
    }
}
