using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Permission
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
