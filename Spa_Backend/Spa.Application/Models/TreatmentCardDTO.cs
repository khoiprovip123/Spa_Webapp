using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class TreatmentCardDTO
    {
        public string? TreatmentCode { get; set; }  //lấy tên dịch vụ + số buổi làm
        public long CustomerID { get; set; }
        public DateTime StartDate { get; set; }
        public string? status { get; set; }
        public string? Notes { get; set; }
        public string CreateBy { get; set; }

        public ICollection<TreatmentDetailDTO> TreatmentDetailDTOs { get; set; }
    }
}
