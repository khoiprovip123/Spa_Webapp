using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface ICustomerRepository
    {
        Customer CreateCustomers(Customer customer);

        IEnumerable<Customer> GetAllCustomer();

        Task<Customer> GetLastCustomerAsync();

        Task<bool> UpdateCustomer(Customer customer);

        Task<bool> DeleteCustomer(Customer customer);

        Customer GetCustomerById(long id);

        Task<Customer> GetCustomerByPhone(string phone, long id);

        Task<Customer> CheckPhoneToCreateCustomer(string phone);

        Task<List<Customer>> GetCustomersFromSpecificCodeAsync(string specificCode);

        Task<bool> DeleteCustomerOnAppoinmentAndSale(long idCustomer);

        Task<List<Customer>> SearchCustomersAsync(string searchTerm);

        Task<string> UploadImageCustomer(CustomerPhoto customerPhoto);

        Task<List<Appointment>> GetHistoryCustomer(long id);

        Task<IEnumerable<Customer>> GetByPages(int pageNumber, int pageSize); // quản lí phân trang

        Task<int> GetAllItemProduct(); //Lấy all số lượng customer
    }
}
