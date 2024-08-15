using Microsoft.Identity.Client;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface ICustomerService
    {
        // List<Customer> GetAllCustomers();
        Task<string> GenerateCustomerCodeAsync();  //generate Id Customer

        IEnumerable<Customer> GetAllCustomer(); //Get All

        Task CreateCustomer(Customer customer); //create

        Task UpdateCustomer(long customerId, Customer customer); //update

        Task DeleteCustomer(long customerId);

        Customer GetCustomerById(long id); //get by id

        bool isExistCustomer(long id); // check customer exist by Id

        Task<bool> GetCustomerByPhone(string phone, long id);

        Task<List<Customer>> SearchCustomersAsync(string searchTerm);  //search

        Task<string> UploadImage(long idCus, string fileName);

        Task<List<Appointment>> GetHistoryCustomerById(long id);

        Task<IEnumerable<Customer>> GetByPages(int pageNumber, int pageSize); // quản lí phân trang

        Task<int> GetAllItem();
    }
}
