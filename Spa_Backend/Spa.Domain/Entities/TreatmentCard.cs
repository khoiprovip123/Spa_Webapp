using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class TreatmentCard
    {
        public long? TreatmentID { get; set; }
        public string? TreatmentCode { get; set; }  //lấy tên dịch vụ + số buổi làm
        public long CustomerID { get; set; }    
        public DateTime? StartDate { get; set; }
        public string? Notes { get; set; }
        public string CreateBy { get; set; }
        public string? Status { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<TreatmentDetail> TreatmentDetails { get; set; }
    }
}
