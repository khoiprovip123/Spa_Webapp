using MediatR;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Spa.Application.Commands
{
    public class CreateAccountCommand : IRequest<string>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, string>
    {
        private readonly IUserService _userService;
        private readonly SpaDbContext _spaDbContext;

        public CreateAccountCommandHandler(IUserService userService, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _spaDbContext = spaDbContext;
        }
        public async Task<string> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                UserName = request.UserName,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
            }; 
            var user = await _userService.GetUserByUserName(account.UserName);
            if (user is not null)
                return null;
            var newAccount = await _userService.CreateAccount(account);
            if (newAccount is null)
            {
                return "Create False!";
            }
            return "Create Success!";
            
        }
    }
}