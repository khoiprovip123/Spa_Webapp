using MediatR;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Infrastructure;

namespace Spa.Application.Commands
{
    public class AddMessageCommand : IRequest<string>
    {
        public string? UserName { get; set; }
        public string? Content { get; set; }
    }
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, string>
    {
        private readonly IMessageService _messService;
        private readonly SpaDbContext _spaDbContext;

        public AddMessageCommandHandler(IMessageService messService, SpaDbContext spaDbContext)
        {
            _messService = messService;
            _spaDbContext = spaDbContext;
        }
        public async Task<string> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Message
            {
                UserName = request.UserName,
                Content = request.Content,
            };
            var addMess = await _messService.AddMessagesAsync(message.UserName, message.Content);
            if (!addMess)
                return "fail";
            return "success";

        }
    }
}
