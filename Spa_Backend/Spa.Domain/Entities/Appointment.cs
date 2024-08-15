using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Appointment
    {
        public long AppointmentID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public long? BranchID { get; set; }
        public long? CustomerID { get; set; }
        public string? Status { get; set; }
        public double? Total { get; set; }
        public Branch Branch { get; set; }
        public Customer Customer { get; set; }
        public double? DiscountAmount { get; set; }
        public int? DiscountPercentage {  get; set; }
        public string? Notes { get; set; }
        public Bill? Bill { get; set; }
        public long? TreatmentID { get; set; }
        public ICollection<ChooseService>? ChooseServices { get; set; }

        public ICollection<CustomerPhoto>? CustomerPhotos { get; set; }

        public ICollection<Assignment>? Assignments { get; set; }

        public ICollection<ChooseServiceTreatment> ChooseServiceTreatments { get; set; }
    }
}
