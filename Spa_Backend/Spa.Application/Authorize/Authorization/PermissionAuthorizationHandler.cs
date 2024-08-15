using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Spa.Application.Authorize.Authorization
{
    public class PermissionAuthorizationHandler:AuthorizationHandler<PermissionRequirment>
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IPermissionService _perService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public PermissionAuthorizationHandler(IPermissionService perService, IMapper mapper, IMediator mediator)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            _perService = perService;
            _mapper = mapper;
            _mediator = mediator;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
        {
            try
            {
                var actorClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor);
                long actorValue = 0;
                long.TryParse(actorClaim.Value, out actorValue);
                var permissions = await _perService.GetAllPermissionNameByJobTypeID(actorValue);
                if (permissions.Contains(requirement.Permission))
                    context.Succeed(requirement);
                else
                    throw new InsufficientPermissionsException("Bạn không có quyền thực hiện hành động này.");
            }
            catch (Exception ex) {
            }
           
        }
    }
}
