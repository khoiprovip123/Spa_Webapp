using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class TreatmentDetail
    {
        public long? TreatmentDetailID { get; set; }
        public long ServiceID { get; set; }
        public double Price { get; set; }   //giá gốc
        public double? AmountDiscount { get; set; } = 0;   //giảm giá
        public string? KindofDiscount { get; set; }  //loại giảm
        public double? TotalAmount { get; set; }  // tiền sau khi giảm
        public string? Note { get; set; } // 
        public int Quantity { get; set; } 
        public int QuantityDone { get; set; }
        public long TreatmentID { get; set; }
        public bool? IsDone { get; set; } = false;
        public ServiceEntity? Service { get; set; }
        public TreatmentCard? TreatmentCard { get; set; }
       public ICollection<ChooseServiceTreatment> ChooseServiceTreatment { get; set; }
    }
}
