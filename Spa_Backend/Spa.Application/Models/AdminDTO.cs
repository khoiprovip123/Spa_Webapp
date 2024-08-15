using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class AdminDTO
    {
        public long? AdminID { get; set; }
        public string? AdminCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Role { get; set; }
        public long? JobTypeID { get; set; }
        public bool? IsActive { get; set; }
        public string? UserName { get; set; }
    }
}
