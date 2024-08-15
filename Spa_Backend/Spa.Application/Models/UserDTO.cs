using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class UserDTO
    {
        public string? Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? UserName {  get; set; } = string.Empty;

        [Required]
        //[EmailAddress]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]

        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]

        public string? ConfirmPassword { get; set; }
        public long? AdminID { get; set; }
        public long? EmployeeID { get; set; }

        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Code { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }
        public long? JobTypeID { get; set; }
        public long? BranchID { get; set; }
        public string? Role { get; set; }
        public bool? isActive { get; set; }
    }
}
