using MediatR;
using Spa.Application.Models;
using Spa.Domain.IService;
using Spa.Infrastructure;

namespace Spa.Application.Commands
{
    public class CreateUserForEmployeeCommand : IRequest<string>
    { 
        public string Email { get; set; }
    }
    public class CreateUserForEmployeeCommandHandler : IRequestHandler<CreateUserForEmployeeCommand, string>
    {
        private readonly IUserService _userService;
        private readonly SpaDbContext _spaDbContext;

        public CreateUserForEmployeeCommandHandler(IUserService userService, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _spaDbContext = spaDbContext;
        }
        public async Task<string> Handle(CreateUserForEmployeeCommand request, CancellationToken cancellationToken)
        {
            /*var emp = await _userService.GetEmpByEmail(request.Email);
            if (emp is null)
            {
                return "Employee not exist";
            }*/
            var user = new UserForEmployeeDTO
            {
                Email = request.Email,
            };
            var newUser = await _userService.CreateUserForEmployee(user.Email);
            if (newUser is null)
            {
                return "";
            }
            return "Create Success!";
        }
    }
}
