using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
        public class AllUsers
        {
            public string Name { get; set; }
            public string? UserCode { get; set; }
            public string? Phone { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? Gender { get; set; }
            public bool? haveAccount { get; set; }
            public bool? isActive { get; set; }
            public string? Status {  get; set; }
        }
}
