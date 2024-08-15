using Spa.Domain.Entities;
using System.Security.Claims;

namespace Spa.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUserName(string userName);
        Task<User> GetUserByUserID(string id);
        Task<string> GetUserBoolByEmail(string email);
        Task<Admin> GetAdminByEmail(string email);
        Task<Employee> GetEmpByEmail(string email);
        Task<List<User>> GetAllUsers();
        Task<List<Employee>> GetAllAdminsAndEmployees();
        Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize);
        Task<IEnumerable<AllUsers>> GetAllUserByPages(int pageNumber, int pageSize);
        Task<IEnumerable<AllUsers>> GetAllAdminByPages(int pageNumber, int pageSize);
        Task<IEnumerable<Account>> GetAllAccountByPages(int pageNumber, int pageSize);
        Task<IEnumerable<Account>> GetAllAccountActiveByPages(int pageNumber, int pageSize);
        Task<IEnumerable<Account>> GetAllAccountNotActiveByPages(int pageNumber, int pageSize);
        Task<IEnumerable<AllUsers>> GetAllUserByPagesAndJobType(int jobTypeId, int pageNumber, int pageSize);
        Task<List<Employee>> GetAllEmployee();
        Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID);
        Task<List<Admin>> GetAllAdmin();
        Task<User> CreateUser(User userDTO);
        Task<Account> CreateAccount(Account accountDTO);
        Task<User> CreateUserForEmployee(string Email);
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        Task<string> LoginAccount(string UserName, string Password);
        Task<string> GenerateToken(string Id, string Name, string Email, long? jobTypeID, string Role);
        Task<string> GenerateRefreshToken();
        Task<(string, string)> RefreshToken(string refreshToken, string jwtToken);
        Task<bool> DeleteUser(string Email);
        Task<bool> UpdateUser(User UserDTO);
        Task<bool> UpdateAccount(Account AccountDTO);
        Task<bool> ChangePassword(Account AccountDTO);
        Task<bool> UpdateAdmin(Admin AdminDTO);
        Task<bool> UpdateEmployee(Employee EmpDTO);
        Task<Admin> GetLastAdminAsync();
        Task<User> GetLastUserAsync();
        Task<Employee> GetLastEmployeeAsync();
        Task<int> GetAllItemProduct();
        Task<int> GetAllItemEmp();
        Task<int> GetAllItemAdmin();
        Task<bool> DeleteAccount(string userName);
        Task<bool> ChangeStatusAccount(string userName);
        Task<List<Employee>> SearchEmployeesAsync(string searchTerm);
    }
}
