using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System.Security.Claims;

namespace Spa.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserRepository userRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<User> CreateUser(User userDTO)
        {
            if (userDTO.Role.Equals("Admin"))
            {
                var admin = await _userRepository.GetAdminByEmail(userDTO.Email);
                long? AdminID = admin.AdminID;
                userDTO.AdminID = AdminID;
                var newUser = await _userRepository.CreateUser(userDTO);
                return newUser;
            }
            else
            {
                var emp = await _userRepository.GetEmpByEmail(userDTO.Email);
                long? empID = emp.EmployeeID;
                userDTO.EmployeeID = empID;
                var newUser = await _userRepository.CreateUser(userDTO);
                return newUser;
            }
        }
        public async Task<Account> CreateAccount(Account accountDTO)
        {
            var lastUserID = await GenerateCodeAsync();
            accountDTO.Code = lastUserID;
            var newUser = await _userRepository.CreateAccount(accountDTO);
            return newUser;
        }
        public async Task<User> CreateUserForEmployee(string Email)
        {
            var newUser = await _userRepository.CreateUserForEmployee(Email);
            return newUser;
        }

        public async Task CreateAdmin(Admin adminDTO)
        {
            var adminCheck = await _userRepository.GetAdminByEmail(adminDTO.Email);
            var empCheck = await _userRepository.GetEmpByEmail(adminDTO.Email);
            if (adminCheck != null || empCheck is not null) { throw new Exception("null"); }
            var lastAdminID = await GenerateAdminCodeAsync();
            adminDTO.AdminCode = lastAdminID;
            await _userRepository.CreateAdmin(adminDTO);
        }

        public async Task CreateEmployee(Employee empDTO)
        {
            var empCheck = await _userRepository.GetEmpByEmail(empDTO.Email);
            var empCheckUser = await _userManager.FindByEmailAsync(empDTO.Email);
            if (empCheck != null || empCheckUser is not null) { throw new Exception(""); }
            var lastEmpID = await GenerateEmployeeCodeAsync();
            empDTO.EmployeeCode = lastEmpID;
            await _userRepository.CreateEmployee(empDTO);
        }

        public async Task DeleteUser(string Email)
        {
            await _userRepository.DeleteUser(Email);
        }

        public async Task<string> GenerateAdminCodeAsync()
        {
            var lastAdminCode = await _userRepository.GetLastAdminAsync();

            if (lastAdminCode == null)
            {
                return "AD0001";
            }
            var lastCode = lastAdminCode.AdminCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "AD" + numericPart.ToString("D4");
        }
        public async Task<string> GenerateCodeAsync()
        {
            var lastUserCode = await _userRepository.GetLastUserAsync();

            if (lastUserCode == null)
            {
                return "AC0001";
            }
            var lastCode = lastUserCode.Code;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "AC" + numericPart.ToString("D4");
        }

        public async Task<string> GenerateEmployeeCodeAsync()
        {
            var lastEmployeeCode = await _userRepository.GetLastEmployeeAsync();

            if (lastEmployeeCode == null)
            {
                return "EM0001";
            }
            var lastCode = lastEmployeeCode.EmployeeCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "EM" + numericPart.ToString("D4");
        }

        public async Task<string> GenerateToken(string Id, string Name, string Email, long? jobTypeID, string Role)
        {
            string token = await _userRepository.GenerateToken(Id, Name, Email, jobTypeID, Role);
            return token;
        }

        public async Task<string> GenerateRefreshToken()
        {
            string refreskToken = await _userRepository.GenerateRefreshToken();
            return refreskToken;
        }

        public async Task<(string, string)> RefreshToken(string refreshToken, string jwtToken)
        {
            return await _userRepository.RefreshToken(refreshToken, jwtToken);
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users;
        }
        public async Task<List<Employee>> GetAllAdminsAndEmployees()
        {
            var employ = await _userRepository.GetAllAdminsAndEmployees();


            return employ;
        }
        public async Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize)
        {
            var listUser = await _userRepository.GetByPages(pageNumber, pageSize);
            return listUser;
        }

        public async Task<IEnumerable<AllUsers>> GetAllUserByPagesAndJobType(int jobTypeId, int pageNumber, int pageSize)
        {
            var listUser = await _userRepository.GetAllUserByPagesAndJobType(jobTypeId,pageNumber, pageSize);
            return listUser;
        }

        public async Task<IEnumerable<AllUsers>> GetAllUserByPages(int pageNumber, int pageSize)
        {
            var listUser = await _userRepository.GetAllUserByPages(pageNumber, pageSize);
            return listUser;
        }
        public async Task<IEnumerable<AllUsers>> GetAllAdminByPages(int pageNumber, int pageSize)
        {
            var listAdmin = await _userRepository.GetAllAdminByPages(pageNumber, pageSize);
            return listAdmin;
        }
        public async Task<IEnumerable<Account>> GetAllAccountByPages(int pageNumber, int pageSize)
        {
            var listAccount = await _userRepository.GetAllAccountByPages(pageNumber, pageSize);
            return listAccount;
        }
        public async Task<IEnumerable<Account>> GetAllAccountActiveByPages(int pageNumber, int pageSize)
        {
            var listAccount = await _userRepository.GetAllAccountActiveByPages(pageNumber, pageSize);
            return listAccount;
        }
        public async Task<IEnumerable<Account>> GetAllAccountNotActiveByPages(int pageNumber, int pageSize)
        {
            var listAccount = await _userRepository.GetAllAccountNotActiveByPages(pageNumber, pageSize);
            return listAccount;
        }
        public async Task<List<Employee>> GetAllEmployee()
        {
            var emps = await _userRepository.GetAllEmployee();
            return emps;
        }
        public async Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID)
        {
            var emps = await _userRepository.GetEmployeeByBranchAndJob(branchID, jobTypeID);
            return emps;
        }
        public async Task<List<Admin>> GetAllAdmin()
        {
            var admins = await _userRepository.GetAllAdmin();
            return admins;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = await _userRepository.GetUserByUserName(userName);
            return user;
        }
        public async Task<User> GetUserByUserID(string id)
        {
            var user = await _userRepository.GetUserByUserID(id);
            return user;
        }
        public async Task<string> GetUserBoolByEmail(string email)
        {
            string checkUser = await _userRepository.GetUserBoolByEmail(email);
            return checkUser;
        }
        public async Task<Admin> GetAdminByEmail(string email)
        {
            var admin = await _userRepository.GetAdminByEmail(email);
            return admin;
        }
        public async Task<Employee> GetEmpByEmail(string email)
        {
            var emp = await _userRepository.GetEmpByEmail(email);
            return emp;
        }

        public async Task<string> LoginAccount(string UserName, string Password)
        {
            string token = await _userRepository.LoginAccount(UserName, Password);
            return token;
        }

        public async Task UpdateAdmin(Admin AdminDTO)
        {
            await _userRepository.UpdateAdmin(AdminDTO);
        }

        public async Task UpdateEmployee(Employee EmpDTO)
        {
            await _userRepository.UpdateEmployee(EmpDTO);
        }

        public async Task UpdateUser(User UserDTO)
        {
            await _userRepository.UpdateUser(UserDTO);
        }

        public async Task UpdateAccount(Account AccountDTO)
        {
            var user = await _userRepository.GetUserByUserName(AccountDTO.UserName);
            if (user is not null && user.Id != AccountDTO.Id)
            {
                throw new Exception("Tài khoản đã tồn tại!");
            }
            await _userRepository.UpdateAccount(AccountDTO);
        }
        public async Task<object> ChangePassword(Account AccountDTO)
        {
            var userUpdate = await _userRepository.GetUserByUserName(AccountDTO.UserName);
            if (userUpdate is null)
                return new
                {
                    flag = false,
                    message = "Tài khoản không tồn tại!"
                };

            bool checkUserPasswords = await _userManager.CheckPasswordAsync(userUpdate, AccountDTO.OldPassword);
            if (!checkUserPasswords)
                return new
                {
                    flag = false,
                    message = "Mật khẩu hiện tại không đúng!"
                };

            bool change = await _userRepository.ChangePassword(AccountDTO);
            if (!change) return new
            {
                flag = change,
                message = "Đổi mật khẩu thất bại!"
            };
            return new
            {
                flag = change,
                message = "Đổi mật khẩu thành công!"
            };
        }
        public bool isExistUser(string Email)
        {
            return _userRepository.GetUserByEmail(Email) == null ? false : true;
        }
        public async Task<int> GetAllItem()
        {
            return await _userRepository.GetAllItemProduct();
        }

        public async Task<int> GetAllItemEmp()
        {
            return await _userRepository.GetAllItemEmp();
        }
        public async Task<int> GetAllItemAdmin()
        {
            return await _userRepository.GetAllItemAdmin();
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public  string GetUserEmail()
        {

            //  var  y = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)?.Value;
            var x = _httpContextAccessor.HttpContext?.User?.Claims;
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public string GetUserBranch()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("Branch")?.Value;
        }

        public async Task<bool> DeleteAccount(string userName)
        {
            return await _userRepository.DeleteAccount(userName);
        }

        public async Task<bool> ChangeStatusAccount(string userName)
        {
            return await _userRepository.ChangeStatusAccount(userName);
        }

        public async Task<List<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            return await _userRepository.SearchEmployeesAsync(searchTerm);
        }
    }
}
