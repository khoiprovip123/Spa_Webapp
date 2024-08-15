using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class AppoinmentDTO
    {
       // public long AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public long? BranchID { get; set; }
        public long CustomerID { get; set; }
        public long EmployeeID { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
    }
}
