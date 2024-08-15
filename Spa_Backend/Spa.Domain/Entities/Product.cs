using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Product
    {
        public long ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public ICollection<Warehouse> Warehouses { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
