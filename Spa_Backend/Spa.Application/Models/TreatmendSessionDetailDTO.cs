using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class TreatmendSessionDetailDTO
    {
        public long SessionID { get; set; }
        public long ServiceID { get; set; } // số dịch vụ chọn trong buổi
        public double Price { get; set; }
        public bool IsDone { get; set; }

    }
}
