using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using Spa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spa.Domain.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            var customers = _customerRepository.GetAllCustomer();
            return customers;
        }

        public async Task<IEnumerable<Customer>> GetByPages(int pageNumber, int pageSize)
        {
            var listCustomer = await _customerRepository.GetByPages(pageNumber, pageSize);
            return listCustomer;
        }


        public async Task CreateCustomer(Customer customer)
        {
            var lastCusID = await GenerateCustomerCodeAsync();
            if (await _customerRepository.CheckPhoneToCreateCustomer(customer.Phone) != null)
            {
                throw new DuplicateException("The phone number already exists in the system.");
            }
            customer.CustomerCode = lastCusID;
            _customerRepository.CreateCustomers(customer);
        }

        public async Task<string> GenerateCustomerCodeAsync()
        {
            var lastCustomerCode = await _customerRepository.GetLastCustomerAsync();

            if (lastCustomerCode == null)
            {
                return "KH0001";
            }
            var lastCode = lastCustomerCode.CustomerCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "KH" + numericPart.ToString("D4");
        }

        public async Task UpdateCustomer(long customerId, Customer customer)
        {
            try
            {
                var customerFromId = _customerRepository.GetCustomerById(customerId);
                bool checckPhone = await GetCustomerByPhone(customer.Phone, customerId);
                if (checckPhone)
                {
                    throw new DuplicateException("The phone number already exists in the system.");
                }
                customerFromId.Gender = customer.Gender;
                customerFromId.FirstName = customer.FirstName;
                customerFromId.LastName = customer.LastName;
                customerFromId.Email = customer.Email;
                customerFromId.Phone = customer.Phone;
                customerFromId.DateOfBirth = customer.DateOfBirth;
                await _customerRepository.UpdateCustomer(customerFromId);
            }
            catch (DuplicateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating customer", ex);
            }
        }

        public Customer GetCustomerById(long id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public bool isExistCustomer(long id)
        {
            return _customerRepository.GetCustomerById(id) == null ? false : true;
        }

        public async Task DeleteCustomer(long customerId)  //Delete cus
        {
            var cusToDelete = GetCustomerById(customerId);
            try
            {
                await _customerRepository.DeleteCustomer(cusToDelete);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                throw new ForeignKeyViolationException("Cannot delete customer because it is referenced by other entities.");
            }
        }

        public async Task<bool> GetCustomerByPhone(string phone, long id)
        {
            bool flag = true;
            var customer = await _customerRepository.GetCustomerByPhone(phone, id);
            if (customer == null)
            {
                flag = false;
            }
            return flag;
        }

        public async Task<bool> CheckPhoneCreateCustomer(string phone)
        {

            return await _customerRepository.CheckPhoneToCreateCustomer(phone) == null ? false : true;
            //bool flag = true;
            //var customer = await _customerRepository.CheckPhoneToCreateCustomer(phone);
            //if (customer == null)
            //{
            //    flag = false;
            //}
            //return flag;
        }

        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        {
            return await _customerRepository.SearchCustomersAsync(searchTerm);
        }

        public async Task<string> UploadImage(long idCus, string fileName)
        {
            CustomerPhoto customerPhoto = new CustomerPhoto
            {
                AppointmentID = idCus,
                PhotoPath = fileName,
            };
            await _customerRepository.UploadImageCustomer(customerPhoto);
            return customerPhoto.PhotoPath;
        }

        public async Task<List<Appointment>> GetHistoryCustomerById(long id)
        {
            return await _customerRepository.GetHistoryCustomer(id);
        }

        public async Task<int> GetAllItem()
        {
          return await _customerRepository.GetAllItemProduct();
        }

    }
}
