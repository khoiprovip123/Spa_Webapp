using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class AppointmentDTO
    {
        public long? AppointmentID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public long? BranchID { get; set; }
        public long? CustomerID { get; set; }
        //  public long EmployeeID { get; set; }
        public string? Status { get; set; }
        public double? Total { get; set; }
        public double? DiscountAmount { get; set; }
        public int? DiscountPercentage { get; set; }
        public CustomerDTO? Customer { get; set; }
        // public EmployeeDTO? Employee { get; set; }
        public ICollection<ChooseServiceDTO>? ChooseServices { get; set; }

        public ICollection<AssignmentDTO>? Assignments { get; set; }

        public string? TeachnicalStaff {get; set;}
        
        public string? Doctor { get; set;}

        public string? SpaTherapist { get; set; }

        public string? EmployeeCode { get; set; }
        
        public string? Notes { get; set;}

        public ICollection<ChooseServiceTreatmentDTO>? ChooseServiceTreatments { get; set; }
    }
}
