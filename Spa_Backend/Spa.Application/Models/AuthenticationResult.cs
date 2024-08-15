using Spa.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Authentication
{
    public record AuthenticationResult(bool? flag, string? mess, UserSession? user, string? Token, string? refreshToken);
}
