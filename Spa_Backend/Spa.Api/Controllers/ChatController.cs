using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Domain.Service;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMediator _mediator;

        public ChatController(IMessageService messageService, IMediator mediator)
        {
            _messageService = messageService;
            _mediator = mediator;
        }
        [HttpGet("allMess")]
        public async Task<ActionResult> GetMessage()
        {
            var listMess = await _messageService.GetMessagesAsync();
            return new JsonResult(listMess);
        }
    }
}
