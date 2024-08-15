using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class EmployeeDTO
    {
        public long? EmployeeID { get; set; }
        public string? EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public long? JobTypeID { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long? BranchID { get; set; }
        public string? Role { get; set; }
        public string? Branch { get; set; }
        public bool? IsActive { get; set; }
    }
}
