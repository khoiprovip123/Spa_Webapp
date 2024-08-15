using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Authentication
{
    public record class AuthenticationResult(User? User, string? Token);
}
