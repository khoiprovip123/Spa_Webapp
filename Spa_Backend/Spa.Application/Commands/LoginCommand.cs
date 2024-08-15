using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spa.Application.Authentication;
using Spa.Application.Models;
using Spa.Domain.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Spa.Application.Commands
{
    public class LoginCommand : IRequest<AuthenticationResult>
    {
        public UserSession Session { get; set; }
        public AuthenticationResult authDTO { get; set; }
        public LoginDTO loginDTO { get; set; }
    }
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IPermissionRepository _permissionRepository;


        public LoginCommandHandler(IUserRepository userRepository, IBranchRepository branchRepository, IPermissionRepository permissionRepository)
        {
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _permissionRepository= permissionRepository;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUserName(request.loginDTO.UserName);
            if (user == null)
            {
                return new AuthenticationResult(false, "Tài khoản không tồn tại!", null, null, null);
            }
            if (!user.IsActiveAcount)
            {
                return new AuthenticationResult(false, "Tài khoản không được phép truy cập!", null, null, null);
            }
            string token = await _userRepository.LoginAccount(request.loginDTO.UserName, request.loginDTO.Password);
            if (token is null)
            {
                return new AuthenticationResult(false, "Mật khẩu không đúng!", null, null, null);
            }
            //var userLogin = await _userRepository.GetUserByEmail(request.loginDTO.UserName);
            if (user.Role != "Admin")
            {
                var emp = await _userRepository.GetEmpByEmail(user.Email);
                var permission = await _permissionRepository.GetAllPermissionNameByJobTypeID(emp.JobTypeID);
                string branch = await _branchRepository.GetBranchNameByID(emp.BranchID);
                var userSession = new UserSession(user.Email,user.UserName, user.LastName + " " + user.FirstName,emp.JobTypeID, user.Role,permission, branch, emp.BranchID, emp.EmployeeCode);
                return new AuthenticationResult(true, "Thành công!", userSession, token, user.RefreshToken);
            }
            else
            {
                var permission = await _permissionRepository.GetAllPermissionNameByJobTypeID(5);
                var userSession = new UserSession(user.Email, user.UserName, user.LastName + " " + user.FirstName,5,user.Role,permission, null, 1, user.Code);
                return new AuthenticationResult(true, "Thành công!", userSession, token, user.RefreshToken);
            }
        }
    }
}
