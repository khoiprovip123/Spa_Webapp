using MediatR;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Infrastructure;

namespace Spa.Application.Commands
{
    public class CreateAdminCommand : IRequest<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, string>
    {
        private readonly IUserService _userService;
        private readonly SpaDbContext _spaDbContext;

        public CreateAdminCommandHandler(IUserService userService, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _spaDbContext = spaDbContext;
        }
        public async Task<string> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = request.Password,
                Email = request.Email,
                UserName = request.UserName,
                Role = "Admin",
                PhoneNumber = request.PhoneNumber,
            };

                var admin = new Admin
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    Phone = user.PhoneNumber,
                    Id = user.Id.ToString(),
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    JobTypeID = 5,
                };
                await _userService.CreateAdmin(admin);
                user.Code = admin.AdminCode;
                var newUser = await _userService.CreateUser(user);
                if (newUser is null)
                {
                    return null;
                }
                return "Create Success!";
        }
    }
}