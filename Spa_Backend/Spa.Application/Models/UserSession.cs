using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public record class UserSession(string? Email,string? UserName, string? Name,long? jobTypeID, string? Role, List<string>? Permissions, string? Branch, long? BranchID, string? userCode);
}
