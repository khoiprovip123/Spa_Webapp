using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Microsoft.AspNetCore.Authorization;
using Spa.Api.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;
        private readonly IBranchService _branchService;
        private readonly IJobService _jobService;

        public UserController(IUserService userService, IMapper mapper, IMediator mediator, IWebHostEnvironment env, IBranchService branchService, IJobService jobService)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _userService = userService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
            _branchService = branchService;
            _jobService = jobService;
        }


        [HttpGet("UserEmail")]
        public async Task<IActionResult> GetUserEmail()
        {
            var email = _userService.GetUserEmail();
            return Ok(email);
        }

        [HttpGet("onlyUser")]
        [HasPermission(SetPermission.OnlyUser)]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpGet("allUser")]
        [HasPermission(SetPermission.ViewAllUser)]
        public async Task<IActionResult> GetAllUser()
        {
            var allUsers = await _userService.GetAllAdminsAndEmployees();
            var admin = await _userService.GetAllAdmin();
            List<Domain.Entities.AllUsers> listAllUser = new List<Domain.Entities.AllUsers>();
            foreach (var i in admin)
            {
                Domain.Entities.AllUsers b = new Domain.Entities.AllUsers
                {
                    Name = i.LastName + " " + i.FirstName,
                    Email = i.Email,
                    Phone = i.Phone,
                    Role = "Quản lý",
                    UserCode = i.AdminCode,
                    Gender = i.Gender,
                    DateOfBirth = i.DateOfBirth,
                    haveAccount = true,
                    isActive = i.IsActive
                };
                if (i.IsActive)
                {
                    listAllUser.Add(b);
                }
            }
            foreach (var i in allUsers)
            {
                Domain.Entities.AllUsers a = new Domain.Entities.AllUsers
                {
                    Name = i.LastName + " " + i.FirstName,
                    Email = i.Email,
                    Phone = i.Phone,
                    Role = i.JobType.JobTypeName,
                    UserCode = i.EmployeeCode,
                    DateOfBirth = i.DateOfBirth,
                    Gender = i.Gender,
                    isActive = i.IsActive
                };
                a.UserCode = i.EmployeeCode;
                string check = await _userService.GetUserBoolByEmail(i.Email);
                a.haveAccount = check == "true";
                if (i.IsActive)
                {
                    listAllUser.Add(a);
                }
            }
            listAllUser = listAllUser
                .OrderByDescending(u => u.Role == "Quản lý")
                .ThenBy(u => u.Role == "Bảo vệ")
                .ThenBy(u => u.Role == "Nhân viên kỹ thuật")
                .ThenBy(u => u.haveAccount is false)
                .ThenBy(u => u.UserCode)
                .ToList();
            return new JsonResult(listAllUser, _jsonSerializerOptions);
        }

        [HttpGet("UserPage")]
        [HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllUserByPage(int pageNumber = 1, int pageSize = 20)
        {
            var userFromService = await _userService.GetByPages(pageNumber, pageSize);

            var userDTO = userFromService.Select(u => new UserDTO
            {
                Code = u.Code,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Phone = u.PhoneNumber,
                Role = u.Role,
            });
            var totalItems = await _userService.GetAllItem();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = userDTO, totalItems, totalPages });
        }

        [HttpGet("AllUserPage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllUserByPages(int pageNumber = 1, int pageSize = 20)
        {
            var userFromService = await _userService.GetAllUserByPages(pageNumber, pageSize);
            var totalItems = await _userService.GetAllItemEmp() + await _userService.GetAllItemAdmin();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = userFromService, totalItems, totalPages });
        }

        [HttpGet("AllEmpPage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllUserByPagesAndJobType(int jobTypeId, int pageNumber = 1, int pageSize = 20)
        {
            var userFromService = await _userService.GetAllUserByPagesAndJobType(jobTypeId,pageNumber, pageSize);
            var totalItems = await _userService.GetAllItemEmp();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = userFromService, totalItems, totalPages, jobTypeId });
        }

        [HttpGet("AllAdminPage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllAdminByPages(int pageNumber = 1, int pageSize = 20)
        {
            var userFromService = await _userService.GetAllAdminByPages(pageNumber, pageSize);
            var totalItems = await _userService.GetAllItemAdmin();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = userFromService, totalItems, totalPages });
        }

        [HttpGet("AllAccountPage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllAccountByPages(int pageNumber = 1, int pageSize = 20)
        {
            var userFromService = await _userService.GetAllAccountByPages(pageNumber, pageSize);
            var totalItems = await _userService.GetAllItem();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = userFromService, totalItems, totalPages });
        }

        [HttpGet("AllAccountActivePage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllAccountActiveByPages(int pageNumber = 1, int pageSize = 20)
        {
            var userFromService = await _userService.GetAllAccountActiveByPages(pageNumber, pageSize);
            var totalItems = await _userService.GetAllItem();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = userFromService, totalItems, totalPages });
        }

        [HttpGet("AllAccountNotActivePage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllAccountNotActiveByPages(int pageNumber = 1, int pageSize = 20)
        {
            var userFromService = await _userService.GetAllAccountNotActiveByPages(pageNumber, pageSize);
            var totalItems = await _userService.GetAllItem();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = userFromService, totalItems, totalPages });
        }


        [HttpGet("allEmployee")]
        [HasPermission(SetPermission.GetAllEmployee)]
        public async Task<IActionResult> GetAllEmployee()
        {
            var allEmps = await _userService.GetAllEmployee();
            return Ok(allEmps);
        }
        [HttpGet("EmployeeByBranchAndJob")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeByBranchAndJob(long branchID, long jobTypeID)
        {
            var allEmps = await _userService.GetEmployeeByBranchAndJob(branchID, jobTypeID);
            return Ok(allEmps);
        }

        [HttpGet("allAdmin")]
        [HasPermission(SetPermission.GetAllAdmin)]
        public async Task<IActionResult> GetAllAdmin()
        {
            var allAdmins = await _userService.GetAllAdmin();
            return Ok(allAdmins);
        }

        [HttpGet("getUserByEmail")]
        [HasPermission(SetPermission.GetUserByEmail)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var getUserByEmail = _userService.GetUserByEmail(email);
            UserDTO userDTO = new UserDTO
            {
                Id = getUserByEmail.Result.Id,
                FirstName = getUserByEmail.Result.FirstName,
                LastName = getUserByEmail.Result.LastName,
                Email = getUserByEmail.Result.Email,
                Code = getUserByEmail.Result.Code,
                Role = getUserByEmail.Result.Role,
                Phone = getUserByEmail.Result.PhoneNumber,
                isActive = getUserByEmail.Result.IsActiveAcount
            };
            return Ok(new { userDTO });
        }

        [HttpGet("getUserByUserName")]
        //[HasPermission(SetPermission.GetUserByEmail)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var user = _userService.GetUserByUserName(userName);
            UserDTO userDTO = new UserDTO
            {
                Id = user.Result.Id,
                FirstName = user.Result.FirstName,
                LastName = user.Result.LastName,
                UserName = user.Result.UserName,
                Email = user.Result.Email,
                Code = user.Result.Code,
                Role = user.Result.Role,
                Phone = user.Result.PhoneNumber,
                isActive = user.Result.IsActiveAcount
            };
            return Ok(new { userDTO });
        }
        [HttpGet("getUserByUserID")]
        //[HasPermission(SetPermission.GetUserByEmail)]
        public async Task<IActionResult> GetUserByUserID(string id)
        {
            var user = _userService.GetUserByUserID(id);
            UserDTO userDTO = new UserDTO
            {
                Id = user.Result.Id.ToString(),
                FirstName = user.Result.FirstName,
                LastName = user.Result.LastName,
                UserName = user.Result.UserName,
                Email = user.Result.Email,
                Code = user.Result.Code,
                Role = user.Result.Role,
                Phone = user.Result.PhoneNumber,
                isActive = user.Result.IsActiveAcount
            };
            return Ok(new { userDTO });
        }

        [HttpGet("getUserByAdmin")]
        [HasPermission(SetPermission.GetUserByAdmin)]
        public async Task<IActionResult> GetAdminByEmail(string email)
        {
            var getAdminByEmail = _userService.GetAdminByEmail(email);
            AdminDTO adminDTO = new AdminDTO
            {
                AdminID = getAdminByEmail.Result.AdminID,
                FirstName = getAdminByEmail.Result.FirstName,
                LastName = getAdminByEmail.Result.LastName,
                Email = getAdminByEmail.Result.Email,
                AdminCode = getAdminByEmail.Result.AdminCode,
                Role = getAdminByEmail.Result.Role,
                DateOfBirth = getAdminByEmail.Result.DateOfBirth,
                Gender = getAdminByEmail.Result.Gender,
                Phone = getAdminByEmail.Result.Phone,
                IsActive = getAdminByEmail.Result.IsActive,
                JobTypeID=getAdminByEmail.Result.JobTypeID,
            };
            return Ok(new { adminDTO });
        }

        [HttpGet("getUserByEmployee")]
        [HasPermission(SetPermission.GetUserByEmployee)]
        public async Task<IActionResult> GetEmpByEmail(string email)
        {
            var getEmpByEmail = _userService.GetEmpByEmail(email);
            EmployeeDTO empDTO = new EmployeeDTO
            {
                EmployeeID = getEmpByEmail.Result.EmployeeID,
                FirstName = getEmpByEmail.Result.FirstName,
                LastName = getEmpByEmail.Result.LastName,
                Email = getEmpByEmail.Result.Email,
                BranchID = getEmpByEmail.Result.BranchID,
                DateOfBirth = getEmpByEmail.Result.DateOfBirth,
                EmployeeCode = getEmpByEmail.Result.EmployeeCode,
                Gender = getEmpByEmail.Result.Gender,
                HireDate = getEmpByEmail.Result.HireDate,
                JobTypeID = getEmpByEmail.Result.JobTypeID,
                Phone = getEmpByEmail.Result.Phone,
                IsActive = getEmpByEmail.Result.IsActive,
                Branch = await _branchService.GetBranchNameByID(getEmpByEmail.Result.BranchID),
            };
            var job = await _jobService.GetJobTypeByID(empDTO.JobTypeID);
            empDTO.Role = job.JobTypeName;
            return Ok(new { empDTO });
        }

        [HttpGet("getUserBoolByEmail")]
        [HasPermission(SetPermission.GetUserBoolByEmail)]
        public async Task<IActionResult> GetUserBoolByEmail(string email)
        {
            string checkUser = await _userService.GetUserBoolByEmail(email);
            return Ok(new { check = checkUser });
        }

        [HttpPut("updateUser")]
        [HasPermission(SetPermission.UpdateUser)]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UpdateDTO updateDto)
        {
            try
            {
                if (updateDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                User user = new User
                {
                    FirstName = updateDto.FirstName,
                    LastName = updateDto.LastName,
                    Role = updateDto.Role,
                    PhoneNumber = updateDto.Phone,
                    Email = email,
                };
                if (user.Role == "Admin")
                {
                    Admin admin = new Admin
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Role = "Admin",
                        DateOfBirth = updateDto.DateOfBirth,
                        Phone = updateDto.Phone,
                        Gender = updateDto.Gender,
                        Email = user.Email,
                        JobTypeID = 5,
                    };
                    await _userService.UpdateUser(user);
                    await _userService.UpdateAdmin(admin);
                    return Ok(true);
                }
                else
                {
                    Employee emp = new Employee
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = updateDto.Gender,
                        HireDate = updateDto.HireDate,
                        Phone = updateDto.Phone,
                        BranchID = updateDto.BranchID,
                        DateOfBirth = updateDto.DateOfBirth,
                        JobTypeID = updateDto.JobTypeID,
                        Email = user.Email
                    };
                    var job = await _jobService.GetJobTypeByID(emp.JobTypeID);
                    user.Role = job.JobTypeName;
                    await _userService.UpdateUser(user);
                    await _userService.UpdateEmployee(emp);
                    return Ok(true);
                }
            }
            catch (DuplicateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("updateAccount")]
        //[HasPermission(SetPermission.UpdateUser)]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] AccountDTO accDto)
        {
            try
            {
                if (accDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Account acc = new Account
                {
                    Id = id,
                    UserName = accDto.UserName,
                    Password = accDto.Password,
                };              
                    await _userService.UpdateAccount(acc);
                    return Ok(true);
            }
            catch (DuplicateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("deleteUser")]
        [HasPermission(SetPermission.DeleteUser)]
        public async Task<ActionResult> DeleteUser(string email)
        {
            try
            {
                await _userService.DeleteUser(email);
                return Ok(true);
            }
            catch (ForeignKeyViolationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("changeStatusAccount")]
        //[HasPermission(SetPermission.DeleteUser)]
        public async Task<ActionResult> ChangeStatusAccount(string userName)
        {
            try
            {
                await _userService.ChangeStatusAccount(userName);
                return Ok(true);
            }
            catch (ForeignKeyViolationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("deleteAccount")]
        //[HasPermission(SetPermission.DeleteUser)]
        public async Task<ActionResult> DeleteAccount(string userName)
        {
            try
            {
                await _userService.DeleteAccount(userName);
                return Ok(true);
            }
            catch (ForeignKeyViolationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("searchEmployee")]
        //[HasPermission(SetPermission.SearchCustomers)]
        public async Task<ActionResult<List<Employee>>> SearchEmployees(string searchTerm)
        {
            try
            {
                var emps = await _userService.SearchEmployeesAsync(searchTerm);
                return Ok(new { emps });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
