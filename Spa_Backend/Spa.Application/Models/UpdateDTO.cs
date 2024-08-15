using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class UpdateDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        [DataType(DataType.Password)]

        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]

        public string? ConfirmPassword { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }
        public long? JobTypeID { get; set; }
        public long? BranchID { get; set; }
        public string? Role { get; set; }
    }
}
