using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class AssignmentDTO
    {
        public long EmployerID { get; set; }
        public long? AppointmentID { get; set; }

        public EmployeeDTO? Employees { get; set; }
    }
}
