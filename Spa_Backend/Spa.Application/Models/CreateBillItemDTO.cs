using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class CreateBillItemDTO
    {
        public long BillItemID { get; set; }
        public long BillID { get; set; }
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice => Quantity * UnitPrice; // Tính tổng giá dựa trên số lượng và đơn giá
    }
}
