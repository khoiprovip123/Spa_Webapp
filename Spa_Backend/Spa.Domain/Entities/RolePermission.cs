using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class RolePermission
    {
        //public long RolePermissionID { get; set; }
        public long JobTypeID { get; set; }
        public long PermissionID { get; set; }
        public JobType? JobTypes { get; set; }
        public Permission? Permissions { get; set; }
    }
}
