using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class AccountDTO
    {
        //public long? Id { get; set; }
        //public string? LastName { get; set; }
        //public string? FirstName { get; set; }

        //[DataType(DataType.EmailAddress)]
        //public string? Email { get; set; }
        public string? UserName { get; set; }
        //public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
        public string? OldPassword {  get; set; }
        //public string? Code { get; set; }
        //public string? Role { get; set; }
        //public bool? IsActiveAcount { get; set; }
    }
}
