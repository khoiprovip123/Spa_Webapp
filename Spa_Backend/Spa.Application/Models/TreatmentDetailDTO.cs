using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class TreatmentDetailDTO
    {
        public long? TreatmentDetailID { get; set; }
        public long ServiceID { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }     
        public int QuantityDone { get; set; }
        public bool? IsDone { get; set; }
        public double? TotalAmount { get; set; }
        public double? AmountDiscount { get; set; } = 0;
        public string? KindofDiscount { get; set; }
        public string? Note { get; set; }
    }
}
