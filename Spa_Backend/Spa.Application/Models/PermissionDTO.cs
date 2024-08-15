using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class PermissionDTO
    {
        public long? PermissionID { get; set; }
        public string PermissionName { get; set; }
        //public IEnumerable<Permission> Permissions { get; set; }
    }
}
