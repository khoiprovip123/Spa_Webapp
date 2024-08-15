using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class UpdateAppointmentDTO
    {
        public DateTime? AppointmentDate { get; set; }
        public long? BranchID { get; set; }
        public long? CustomerID { get; set; }
        public string? Status { get; set; }
        public double? Total { get; set; }
        public double? DiscountAmount { get; set; }
        public int? DiscountPercentage { get; set; }
        public string? Notes { get; set; }

        public List<long>? ListServiceID { get; set; }

        public ICollection<ChooseServiceTreatmentDTO>? ChooseServiceTreatmentDTO { get; set; }
    }
}
