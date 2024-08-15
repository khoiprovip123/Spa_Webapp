using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Branch
    {
        public long BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchPhone { get; set; }

        
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

        public ICollection<Warehouse> Warehouse { get; set; }
    }
}
