using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Spa.Application.Authorize.Permissions;

namespace Spa.Application.Authorize.HasPermissionAbtribute
{
    public sealed class HasPermissionAttribute:AuthorizeAttribute
    {
        public HasPermissionAttribute(SetPermission permission) : base(policy:Convert.ToString(permission))
        {
            
        }
    }
}
