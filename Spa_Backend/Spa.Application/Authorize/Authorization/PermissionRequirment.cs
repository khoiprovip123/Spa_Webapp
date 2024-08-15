using Microsoft.AspNetCore.Authorization;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Authorize.Authorization
{
    public class PermissionRequirment:IAuthorizationRequirement
    {
        public PermissionRequirment(string permission) {
            Permission = permission;
        }
        public string Permission { get; }
    }
    
}
