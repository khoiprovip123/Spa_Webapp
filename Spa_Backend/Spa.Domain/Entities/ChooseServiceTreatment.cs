using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class ChooseServiceTreatment
    {
        public int ID { get; set; }
        public long AppointmentID { get; set; }
        public long TreatmentDetailID { get; set; }
        public int QualityChooses { get; set; }
        public Appointment? Appointment { get; set; }
        public TreatmentDetail? TreatmentDetail { get; set; }
    }
}
