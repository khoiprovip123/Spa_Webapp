using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NMemory.Linq;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Spa.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private static readonly List<User> _user = new();
        private static readonly List<Admin> _admin = new();
        private static readonly List<Employee> _employee = new();
        private readonly IPermissionRepository _per;
        private readonly IJobRepository _job;
        private readonly IConfiguration _config;
        private readonly SpaDbContext _spaDbContext;
        public UserRepository(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SpaDbContext spaDbContext, IConfiguration config
            , IPermissionRepository per, IJobRepository job
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _spaDbContext = spaDbContext;
            _per = per;
            _job = job;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }
        public async Task<User> GetUserByUserID(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<string> GetUserBoolByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                return "true";
            }
            return "false";
        }
        public async Task<Admin> GetAdminByEmail(string email)
        {
            var admin = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == email);
            return admin;
        }
        public async Task<Employee> GetEmpByEmail(string email)
        {
            var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
            return emp;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users is null)
            {
                return null;
            }

            var userDTOs = users.Select(user => new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Code = user.Code,
                IsActiveAcount = user.IsActiveAcount,
            }).OrderBy(u => u.Code).ToList();

            return userDTOs;
        }
        public async Task<List<Employee>> GetAllAdminsAndEmployees()
        {
            var user = await _spaDbContext.Employees.OrderBy(u => u.EmployeeCode).Include(j => j.JobType).ToListAsync();

            return user;
        }

        public async Task<int> GetAllItemProduct()
        {
            return await _userManager.Users.CountAsync();
        }
        public async Task<int> GetAllItemEmp()
        {
            return await _spaDbContext.Employees.CountAsync();
        }
        public async Task<int> GetAllItemAdmin()
        {
            return await _spaDbContext.Admins.CountAsync();
        }
        public async Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize)
        {
            return await _userManager.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<AllUsers>> GetAllUserByPages(int pageNumber, int pageSize)
        {
            var listAllUser = new List<AllUsers>();
            var adminUsers = await (
           from a in _spaDbContext.Admins
           join jt in _spaDbContext.JobTypes on a.JobTypeID equals jt.JobTypeID
           from u in _spaDbContext.Users.Where(u => u.Code == a.AdminCode).DefaultIfEmpty()
           /*where a.IsActive*/
           select new AllUsers
           {
               Name = a.LastName + " " + a.FirstName,
               UserCode = a.AdminCode,
               Phone = a.Phone,
               Role = "Quản lý",
               Email = a.Email,
               DateOfBirth = a.DateOfBirth,
               Gender = a.Gender,
               haveAccount = u != null,
               isActive = a.IsActive,
               Status = a.IsActive == true ? "Đang hoạt động" : "Ngừng hoạt động",
           }).ToListAsync();
            listAllUser.AddRange(adminUsers);

            var employeeUsers = await (
           from e in _spaDbContext.Employees
           join jt in _spaDbContext.JobTypes on e.JobTypeID equals jt.JobTypeID
           from u in _spaDbContext.Users.Where(u => u.Code == e.EmployeeCode).DefaultIfEmpty()
           /*where e.IsActive*/
           select new AllUsers
           {
               Name = e.LastName + " " + e.FirstName,
               UserCode = e.EmployeeCode,
               Phone = e.Phone,
               Role = jt.JobTypeName,
               Email = e.Email,
               DateOfBirth = e.DateOfBirth,
               Gender = e.Gender,
               haveAccount = u != null,
               isActive = e.IsActive,
               Status = e.IsActive == true ? "Đang hoạt động" : "Ngừng hoạt động",
           }).ToListAsync();
            listAllUser.AddRange(employeeUsers);

            listAllUser = listAllUser
                    .OrderByDescending(u => u.Role == "Quản lý")
                    .ThenBy(u => u.Role == "Bảo vệ")
                    .ThenBy(u => u.Role == "Nhân viên kỹ thuật")
                    .ThenBy(u => u.haveAccount is false)
                    .ThenBy(u => u.UserCode)
                    .ToList();
            return listAllUser.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<AllUsers>> GetAllAdminByPages(int pageNumber, int pageSize)
        {
            var listAllUser = new List<AllUsers>();
            var adminUsers = await _spaDbContext.Admins
                .Where(a => a.IsActive)
                .Select(a => new AllUsers
                {
                    Name = a.LastName + " " + a.FirstName,
                    UserCode = a.AdminCode,
                    Phone = a.Phone,
                    Role = "Quản lý",
                    Email = a.Email,
                    DateOfBirth = a.DateOfBirth,
                    Gender = a.Gender,
                    haveAccount = true,
                    isActive = a.IsActive
                }).ToListAsync();
            listAllUser.AddRange(adminUsers);

            listAllUser = listAllUser
                    .OrderBy(u => u.UserCode)
                    .ToList();
            return listAllUser.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Account>> GetAllAccountByPages(int pageNumber, int pageSize)
        {
            var listAllAccount = new List<Account>();
            var account = await _userManager.Users
                .Select(u => new Account
                {
                    
                    Code = u.Code,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    Role = u.Role,
                    Email = u.Email,
                    UserName = u.UserName,                  
                    PhoneNumber = u.PhoneNumber,
                    IsActiveAcount = u.IsActiveAcount == true ? "Đang hoạt động" : "Ngừng hoạt động",
                    //Id = u.Role == "Admin" ? u.AdminID : u.EmployeeID,
                    Id = u.Id.ToString(),
                }).ToListAsync();

            listAllAccount.AddRange(account);

            listAllAccount = listAllAccount
                //.OrderByDescending(u => u.IsActiveAcount == "Đang hoạt động")
                //.ThenBy(u => u.Code)
                .OrderBy(u=>u.Code)
                .ToList();
            return listAllAccount.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Account>> GetAllAccountActiveByPages(int pageNumber, int pageSize)
        {
            var listAllAccount = new List<Account>();
            var account = await _userManager.Users
                .Where(u => u.IsActiveAcount)
                .Select(u => new Account
                {

                    Code = u.Code,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    Role = u.Role,
                    Email = u.Email,
                    UserName = u.UserName,
                    PhoneNumber = u.PhoneNumber,
                    IsActiveAcount = "Đang hoạt động",
                    Id = u.Id.ToString(),
                }).ToListAsync();

            listAllAccount.AddRange(account);

            listAllAccount = listAllAccount
                    .OrderBy(u => u.Code)
                    .ToList();
            return listAllAccount.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Account>> GetAllAccountNotActiveByPages(int pageNumber, int pageSize)
        {
            var listAllAccount = new List<Account>();
            var account = await _userManager.Users
                .Where(u => !u.IsActiveAcount)
                .Select(u => new Account
                {

                    Code = u.Code,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    Role = u.Role,
                    Email = u.Email,
                    UserName = u.UserName,
                    PhoneNumber = u.PhoneNumber,
                    IsActiveAcount = "Ngừng hoạt động",
                    Id = u.Id.ToString(),
                }).ToListAsync();

            listAllAccount.AddRange(account);

            listAllAccount = listAllAccount
                    .OrderBy(u => u.Code)
                    .ToList();
            return listAllAccount.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<AllUsers>> GetAllUserByPagesAndJobType(int jobTypeId,int pageNumber, int pageSize)
        {
            var listAllUserByJob = new List<AllUsers>();
            var employeeUsers = await (
           from e in _spaDbContext.Employees
           join jt in _spaDbContext.JobTypes on e.JobTypeID equals jt.JobTypeID
           from u in _spaDbContext.Users.Where(u => u.Code == e.EmployeeCode).DefaultIfEmpty()
           where e.IsActive && e.JobTypeID == jobTypeId
           select new AllUsers
           {
               Name = e.LastName + " " + e.FirstName,
               UserCode = e.EmployeeCode,
               Phone = e.Phone,
               Role = jt.JobTypeName,
               Email = e.Email,
               DateOfBirth = e.DateOfBirth,
               Gender = e.Gender,
               haveAccount = u != null,
               isActive = e.IsActive
           }).ToListAsync();
            listAllUserByJob.AddRange(employeeUsers);

            listAllUserByJob = listAllUserByJob
                    .OrderBy(u => u.UserCode)
                    .ToList();
            return listAllUserByJob.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }


        public async Task<List<Employee>> GetAllEmployee()
        {
            var emps = await _spaDbContext.Employees
                .ToListAsync();
            if (emps is null)
            {
                return null;
            }
            var empDTOs = emps.Select(emp => new Employee
            {
                EmployeeCode = emp.EmployeeCode,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                Phone = emp.Phone,
                Gender = emp.Gender,
                BranchID = emp.BranchID,
                DateOfBirth = emp.DateOfBirth,
                HireDate = emp.HireDate,
                Assignments = emp.Assignments,
                JobTypeID = emp.JobTypeID,
                IsActive = emp.IsActive,
            }).OrderBy(e => e.EmployeeCode).ToList();
            return empDTOs;
        }

        public async Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID)
        {
            var emps = await _spaDbContext.Employees
                .Where(e => e.BranchID == branchID && e.JobTypeID == jobTypeID && e.IsActive)
                .ToListAsync();
            if (emps is null || !emps.Any())
            {
                return null;
            }

            var empDTOs = emps.Select(emp => new Employee
            {
                EmployeeID = emp.EmployeeID,
                EmployeeCode = emp.EmployeeCode,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                Phone = emp.Phone,
                Gender = emp.Gender,
                BranchID = emp.BranchID,
                DateOfBirth = emp.DateOfBirth,
                HireDate = emp.HireDate,
                Assignments = emp.Assignments,
                JobTypeID = emp.JobTypeID,
                IsActive = emp.IsActive,
            }).OrderBy(e => e.EmployeeCode).ToList();
            return empDTOs;
        }
        public async Task<List<Admin>> GetAllAdmin()
        {
            var admins = await _spaDbContext.Admins.ToListAsync();
            if (admins is null)
            {
                return null;
            }

            var adminDTOs = admins.Select(admin => new Admin
            {
                AdminCode = admin.AdminCode,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Phone = admin.Phone,
                Gender = admin.Gender,
                DateOfBirth = admin.DateOfBirth,
                IsActive = admin.IsActive,
            }).OrderBy(a => a.AdminCode).ToList();

            return adminDTOs;
        }

        public async Task<User> CreateUser(User userDTO)
        {
            if (userDTO is null) return null;
            var newUser = new User()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                PasswordHash = userDTO.PasswordHash,
                Role = userDTO.Role,
                Code = userDTO.Code,
                UserName = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                AdminID = userDTO.AdminID,
                EmployeeID = userDTO.EmployeeID,
                IsActiveAcount = true,
            };
            var createUser = await _userManager.CreateAsync(newUser!, userDTO.PasswordHash);
            if (!createUser.Succeeded)
            {
                throw null;
            }
            return newUser;
        }
        public async Task<Account> CreateAccount(Account accountDTO)
        {
            if (accountDTO is null) return null;
            var newUser = new User()
            {
                LastName = accountDTO.LastName ?? "Chưa có",
                FirstName = accountDTO.FirstName ?? "Chưa có",
                Email = accountDTO.UserName,
                PasswordHash = accountDTO.Password,
                Role = accountDTO.Role ?? "Chưa có",
                Code = accountDTO.Code ?? "Chưa có",
                UserName = accountDTO.UserName,
                PhoneNumber = accountDTO.PhoneNumber ?? "Chưa có",
                IsActiveAcount = true,
            };
            try
            {
                var createUser = await _userManager.CreateAsync(newUser!, accountDTO.Password);
                if (!createUser.Succeeded)
                {
                    throw null;
                }
                return accountDTO;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public async Task<User> CreateUserForEmployee(string Email)
        {
            var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == Email);
            //if (emp is null) return null;
            if (emp is null)
            {
                var ad = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == Email);
                {
                    var newAdmin = new User()
                    {
                        FirstName = ad.FirstName,
                        LastName = ad.LastName,
                        Email = ad.Email,
                        PasswordHash = "Spa@12345",
                        Code = ad.AdminCode,
                        UserName = ad.Email,
                        PhoneNumber = ad.Phone,
                        AdminID = ad.AdminID,
                        Id = ad.Id,
                        Role="Admin",
                        IsActiveAcount = true
                    };
                    var createUserForAdmin = await _userManager.CreateAsync(newAdmin!, newAdmin.PasswordHash);
                    if (!createUserForAdmin.Succeeded)
                    {
                        return null;
                    }
                    return newAdmin;
                }
            }
            var newUser = new User()
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                PasswordHash = "Spa@12345",
                Code = emp.EmployeeCode,
                UserName = emp.Email,
                PhoneNumber = emp.Phone,
                EmployeeID = emp.EmployeeID,
                Id = emp.Id,
                IsActiveAcount = true
            };
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user is not null)
            {
                return null;
            }
            var userRole = await _spaDbContext.JobTypes.FindAsync(emp.JobTypeID);
            newUser.Role = userRole.JobTypeName;
            var createUser = await _userManager.CreateAsync(newUser!, newUser.PasswordHash);
            if (!createUser.Succeeded)
            {
                return null;
            }
            return newUser;
        }
        public async Task CreateAdmin(Admin adminDTO)
        {
            var newAdmin = new Admin()
            {
                Email = adminDTO.Email,
                FirstName = adminDTO.FirstName,
                LastName = adminDTO.LastName,
                Role = adminDTO.Role,
                AdminCode = adminDTO.AdminCode,
                Id = adminDTO.Id,
                Phone = adminDTO.Phone,
                DateOfBirth = adminDTO.DateOfBirth,
                Gender = adminDTO.Gender,
                JobTypeID = 5,
            };
            await _spaDbContext.Admins.AddAsync(newAdmin);
            await _spaDbContext.SaveChangesAsync();
        }

        public async Task CreateEmployee(Employee empDTO)
        {
            var newEmployee = new Employee()
            {
                Email = empDTO.Email,
                FirstName = empDTO.FirstName,
                LastName = empDTO.LastName,
                EmployeeCode = empDTO.EmployeeCode,
                Id = empDTO.Id,
                Phone = empDTO.Phone,
                DateOfBirth = empDTO.DateOfBirth,
                Gender = empDTO.Gender,
                HireDate = empDTO.HireDate,
                JobTypeID = empDTO.JobTypeID,
                BranchID = empDTO.BranchID
            };
            await _spaDbContext.Employees.AddAsync(newEmployee);
            await _spaDbContext.SaveChangesAsync();
        }

        public async Task<string> LoginAccount(string UserName, string Password)
        {
            if (UserName is null || Password is null)
                return "Empty";

            var getUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == UserName); ;
            if (getUser is null)
                return "User not exist";
            bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, Password);
            if (!checkUserPasswords)
                return null;
            getUser.RefreshToken = await GenerateRefreshToken();
            getUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(getUser);
            if (getUser.Role.Equals("Admin"))
            {
                string token = await GenerateToken(getUser.Code, (getUser.FirstName + " " + getUser.LastName), getUser.Email, 5, getUser.Role);
                return token;
            }
            else
            {
                var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == getUser.Email);
                string token = await GenerateToken(getUser.Code, (getUser.FirstName + " " + getUser.LastName), getUser.Email, emp.JobTypeID, getUser.Role);
                return token;
            }
        }

        public async Task<string> GenerateToken(string Id, string Name, string Email, long? jobTypeID, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            //claim information
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Id),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Role, Role),
                new Claim(ClaimTypes.Actor, jobTypeID.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string jwtToken)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }

        public async Task<(string, string)> RefreshToken(string refreshToken, string jwtToken)
        {
            var principal = GetPrincipalFromExpiredToken(jwtToken);
            if (principal?.FindFirst(ClaimTypes.Email) is null)
                return (null, null);
            string Email = principal?.FindFirst(ClaimTypes.Email).Value;
            var user = await GetUserByEmail(Email);
            if (user is null || user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime < DateTime.Now)
                return (null, null);
            if (user.Role.Equals("Admin"))
            {
                string userToken = await GenerateToken(user.Code, (user.FirstName + " " + user.LastName), user.Email, 1, user.Role);
                user.RefreshToken = await GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _userManager.UpdateAsync(user);
                return (user.RefreshToken, userToken);
            }
            else
            {
                var emp = await GetEmpByEmail(Email);
                string userToken = await GenerateToken(user.Code, (user.FirstName + " " + user.LastName), user.Email, emp.JobTypeID, user.Role);
                user.RefreshToken = await GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _userManager.UpdateAsync(user);
                return (user.RefreshToken, userToken);
            }
        }

        public async Task<bool> DeleteUser(string Email)
        {
            var admin = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == Email);
            if (admin != null)
            {
                admin.IsActive = !admin.IsActive;
                _spaDbContext.Admins.Update(admin);
                _spaDbContext.SaveChanges();
                return true;
            }
            else
            {
                var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == Email);
                if (emp.Email is null) return false;
                {
                    emp.IsActive = !emp.IsActive;
                    _spaDbContext.Employees.Update(emp);
                    _spaDbContext.SaveChanges();
                    return true;
                }
            }
        }

        public async Task<bool> UpdateUser(User UserDTO)
        {
            var newUpdate = new User
            {
                FirstName = UserDTO.FirstName,
                LastName = UserDTO.LastName,
                Email = UserDTO.Email,
                PasswordHash = UserDTO.PasswordHash,
                Role = UserDTO.Role,
                PhoneNumber = UserDTO.PhoneNumber,
            };
            var userUpdate = await _userManager.FindByEmailAsync(UserDTO.Email);
            if (userUpdate is null) return false;
            userUpdate.FirstName = newUpdate.FirstName;
            userUpdate.LastName = newUpdate.LastName;
            userUpdate.Role = newUpdate.Role;
            userUpdate.PhoneNumber = newUpdate.PhoneNumber;
            var updateUserResult = await _userManager.UpdateAsync(userUpdate);
            if (!updateUserResult.Succeeded)
                return false;
            return true;
        }

        public async Task<bool> UpdateAccount(Account AccountDTO)
        {
            var newUpdate = new User
            {
                Id= AccountDTO.Id,
                UserName = AccountDTO.UserName,
                PasswordHash = AccountDTO.Password,
            };
            var userUpdate = await GetUserByUserID(AccountDTO.Id);
            if (userUpdate is null) return false;

            userUpdate.UserName = newUpdate.UserName;

            var passwordHasher = new PasswordHasher<User>();
            userUpdate.PasswordHash = passwordHasher.HashPassword(userUpdate, newUpdate.PasswordHash);

            var updateUserResult = await _userManager.UpdateAsync(userUpdate);
            if (!updateUserResult.Succeeded)
                return false;
            return true;
        }

        public async Task<bool> ChangePassword(Account AccountDTO)
        {
            var newUpdate = new User
            {
                UserName = AccountDTO.UserName,
                PasswordHash = AccountDTO.Password,
            };
            var userUpdate = await GetUserByUserName(AccountDTO.UserName);
            var passwordHasher = new PasswordHasher<User>();
            userUpdate.PasswordHash = passwordHasher.HashPassword(userUpdate, newUpdate.PasswordHash);
            var updateUserResult = await _userManager.UpdateAsync(userUpdate);
            if (!updateUserResult.Succeeded)
                return false;
            return true;
        }

        public async Task<bool> UpdateAdmin(Admin AdminDTO)
        {
            var newUpdate = new Admin
            {
                FirstName = AdminDTO.FirstName,
                LastName = AdminDTO.LastName,
                Email = AdminDTO.Email,
                Phone = AdminDTO.Phone,
                DateOfBirth = AdminDTO.DateOfBirth,
                Gender = AdminDTO.Gender,
                JobTypeID = 5,
                Role = "Admin",
            };
            var adminUpdate = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == newUpdate.Email);
            if (adminUpdate is null) return false;
            {
                adminUpdate.FirstName = newUpdate.FirstName;
                adminUpdate.LastName = newUpdate.LastName;
                adminUpdate.Email = newUpdate.Email;
                adminUpdate.Phone = newUpdate.Phone;
                adminUpdate.DateOfBirth = newUpdate.DateOfBirth;
                adminUpdate.Gender = newUpdate.Gender;
                adminUpdate.Role = newUpdate.Role;
                adminUpdate.JobTypeID = newUpdate.JobTypeID;
            }
            _spaDbContext.Admins.Update(adminUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }
        public async Task<bool> UpdateEmployee(Employee EmpDTO)
        {
            var newUpdate = new Employee
            {
                FirstName = EmpDTO.FirstName,
                LastName = EmpDTO.LastName,
                Email = EmpDTO.Email,
                Phone = EmpDTO.Phone,
                DateOfBirth = EmpDTO.DateOfBirth,
                Gender = EmpDTO.Gender,
                HireDate = EmpDTO.HireDate,
                JobTypeID = EmpDTO.JobTypeID,
                BranchID = EmpDTO.BranchID,
            };
            var empUpdate = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == newUpdate.Email);
            if (empUpdate is null) return false;
            {
                empUpdate.FirstName = newUpdate.FirstName;
                empUpdate.LastName = newUpdate.LastName;
                empUpdate.Email = newUpdate.Email;
                empUpdate.Phone = newUpdate.Phone;
                empUpdate.DateOfBirth = newUpdate.DateOfBirth;
                empUpdate.Gender = newUpdate.Gender;
                empUpdate.HireDate = newUpdate.HireDate;
                empUpdate.JobTypeID = newUpdate.JobTypeID;
                empUpdate.BranchID = newUpdate.BranchID;
            }
            _spaDbContext.Employees.Update(empUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<Admin> GetLastAdminAsync()
        {
            try
            {
                return await _spaDbContext.Admins.OrderByDescending(a => a.AdminCode).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<User> GetLastUserAsync()
        {
            try
            {
                return await _userManager.Users
                              .Where(u => u.Code.StartsWith("AC"))
                              .OrderByDescending(u => u.Code)
                              .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Employee> GetLastEmployeeAsync()
        {
            try
            {
                return await _spaDbContext.Employees.OrderByDescending(e => e.EmployeeCode).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAccount(string userName)
        {
            var user = await GetUserByUserName(userName);
            if (user is not null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> ChangeStatusAccount(string userName)
        {
            var user = await GetUserByUserName(userName);
            if (user != null)
            {
                user.IsActiveAcount = !user.IsActiveAcount;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<List<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            return await _spaDbContext.Employees
                .Where(c => (c.LastName + " " + c.FirstName).Contains(searchTerm) || (c.LastName + c.FirstName).Contains(searchTerm)
                || c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm) || c.Phone.Contains(searchTerm))
                .ToListAsync();
        }
    }
}