using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class ChooseServiceTreatmentDTO
    {
        public long AppointmentID { get; set; }
        public long TreatmentDetailID { get; set; }
        public int QualityChooses { get; set; }
        public TreatmentDetailDTO? TreatmentDetail { get; set; }
    }
}
