using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Admin
    {
        public long? AdminID { get; set; }
        public string Id { get; set; }
        public string? AdminCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; } = true;
        public long? JobTypeID { get; set; } = 5;
        public ICollection<User>? User { get; set; }
        public JobType? JobType { get; set; }
    }
}
