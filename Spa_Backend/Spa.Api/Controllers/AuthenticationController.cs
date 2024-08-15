using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Authentication;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Infrastructure;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly SpaDbContext _spaDbContext;

        public AuthenticationController(IUserService userService, IMapper mapper, IMediator mediator, IWebHostEnvironment env, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
            _spaDbContext = spaDbContext;
        }

        [HttpGet("getClaims"), Authorize]
        public JsonResult GetClaims()
        {
            var claimValues = new
            {
                UserCode = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Name = User.FindFirst(ClaimTypes.Name)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value,
                Actor = User.FindFirst(ClaimTypes.Actor)?.Value,
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 64
            };
            return new JsonResult(claimValues, jsonSerializerOptions);
        }

        [HttpPost("refreshToken"), Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO tokenDTO)
        {
            try
            {
                var token = new TokenDTO
                {
                    AccessToken = tokenDTO.AccessToken,
                    RefreshToken = tokenDTO.RefreshToken
                };
                var refresh = await _userService.RefreshToken(token.RefreshToken, token.AccessToken);
                return Ok(new { refresh.Item1, refresh.Item2 });
            }
            catch (Exception ex)
            {
                return Ok(new { });
            }
        }

        [HttpPost("register")]
        [HasPermission(SetPermission.CreateUser)]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new RegisterCommand
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    Role = userDto.Role,
                    PhoneNumber = userDto.Phone,
                    Gender = userDto.Gender,
                    DateOfBirth = userDto.DateOfBirth,
                    HireDate = userDto.HireDate,
                    JobTypeID = userDto.JobTypeID,
                    BranchID = userDto.BranchID,
                };
                var id = await _mediator.Send(command);
                return Ok(new { status = id });
            }
            catch (DuplicateException ex1)
            {
                return Ok(new { });
            }
            catch (Exception ex2)
            {
                return Ok(new { });
            }
        }

        [HttpPost("changePassword")]
        //[HasPermission(SetPermission.CreateUser)]
        public async Task<IActionResult> changePassword([FromBody] AccountDTO accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var account = new Account
                {
                    UserName = accountDto.UserName,
                    OldPassword = accountDto.OldPassword,
                    Password = accountDto.Password,
                };
                var id = await _userService.ChangePassword(account);
                return Ok(id);
            }
            catch (DuplicateException ex1)
            {
                return Ok(new { });
            }
            catch (Exception ex2)
            {
                return Ok(new { });
            }
        }

        [HttpPost("createAdmin")]
        //[HasPermission(SetPermission.CreateUser)]
        public async Task<IActionResult> CreateAdmin([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new CreateAdminCommand
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    UserName= userDto.UserName,
                    Password = userDto.Password,
                    PhoneNumber = userDto.Phone,
                    Gender = userDto.Gender,
                    DateOfBirth = userDto.DateOfBirth,
                };
                var id = await _mediator.Send(command);
                return Ok(new { status = id });
            }
            catch (DuplicateException ex1)
            {
                return Ok(new { });
            }
            catch (Exception ex2)
            {
                return Ok(new { });
            }
        }

        [HttpPost("createAccount")]
        //[HasPermission(SetPermission.CreateUser)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDTO accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new CreateAccountCommand
                {
                    Password = accountDto.Password,
                    UserName = accountDto.UserName,
                    ConfirmPassword = accountDto.ConfirmPassword,
                };
                var id = await _mediator.Send(command);
                return Ok(new { status = id });
            }
            catch (DuplicateException ex1)
            {
                return Ok(new { });
            }
            catch (Exception ex2)
            {
                return Ok(new { });
            }
        }

        [HttpPost("CreateUserForEmployee")]
        [HasPermission(SetPermission.CreateUserForEmployee)]
        public async Task<IActionResult> CreateUserForEmployee([FromBody] UserForEmployeeDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var command = new CreateUserForEmployeeCommand
                {
                    Email = userDTO.Email,
                };
                var id = await _mediator.Send(command);
                return Ok(new { status = id });
            }
            catch (DuplicateException ex)
            {
                return Ok(new { });
            }
            catch (Exception ex)
            {
                return Ok(new { });
            }
        }


        [HttpPost("login")]
        public async Task<AuthenticationResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return new AuthenticationResult(false, "Empty", null, null, null);
            }
            try
            {
                var command = new LoginCommand
                {
                    loginDTO = loginDto
                };
                var id = await _mediator.Send(command);
                return id;
            }
            catch (DuplicateException ex)
            {
                return new AuthenticationResult(false, "Error", null, null, null);
            }
            catch (Exception cannotLogin)
            {
                return new AuthenticationResult(false, "Tài khoản không được phép truy cập!", null, null, null);
            }
        }

        [HttpGet("getCookies")]
        public IActionResult GetUserFromCookie()
        {
            var userJson = Request.Cookies["User"];
            if (userJson != null)
            {
                var user = JsonSerializer.Deserialize<Domain.Entities.AllUsers>(userJson);
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("logOut")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("User");
            return Ok();
        }
    }
}
