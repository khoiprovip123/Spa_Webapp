using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string? Code { get; set; }
        public long? EmployeeID { get; set; }
        public long? AdminID { get; set; }
        public Admin Admin { get; set; }
        public Employee Employee { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }


        public bool IsActiveAcount { get; set; }
    }
}
