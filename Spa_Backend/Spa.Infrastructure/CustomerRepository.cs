using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructures
{
    public class CustomerRepository : EfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public Customer CreateCustomers(Customer customer)
        {
            Add(customer);
            return customer;
        }

        public async Task<bool> DeleteCustomer(Customer customer) //delete customer
        {
            if (customer != null)
            {
                // customer.IsActive = false;
                // Update(customer);
                DeleteById(customer);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCustomerOnAppoinmentAndSale(long idCustomer)
        {
            try
            {
                var appointment = await _spaDbContext.Appointments.Where(a => a.CustomerID == idCustomer).ToListAsync();
                // var chooseService = _spaDbContext.ChooseServices.Where(c => c.ServiceID.)
                var sale = await _spaDbContext.Sales.Where(s => s.CustomerID == idCustomer).ToListAsync();
                if (appointment != null || sale != null)
                {
                    _spaDbContext.Appointments.RemoveRange(appointment);
                    _spaDbContext.Sales.RemoveRange(sale);
                }
                await _spaDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return true;
            }
            return true;
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            return GetAll();
        }

        public async Task<IEnumerable<Customer>> GetByPages(int pageNumber, int pageSize)
        {
            return await _spaDbContext.Customers.OrderBy(i => i.CustomerCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> GetAllItemProduct()
        {
            return await _spaDbContext.Customers.CountAsync();
        }


        public Customer GetCustomerById(long id) //get customer by Id
        {
            return GetById(id);
        }

        public async Task<Customer> GetCustomerByPhone(string phone, long id)  //get customer by phone
        {
            // Customer cus = null;
            return await _spaDbContext.Customers.FirstOrDefaultAsync(c => c.Phone == phone && id != c.CustomerID);
        }

        public async Task<Customer> CheckPhoneToCreateCustomer(string phone)  //check phone
        {
            return await _spaDbContext.Customers.FirstOrDefaultAsync(c => c.Phone == phone);
        }

        public async Task<Customer> GetLastCustomerAsync()
        {
            try
            {
                return await _spaDbContext.Customers.OrderByDescending(c => c.CustomerID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateCustomer(Customer customer)  //update customer
        {
            Update(customer);
            return true;
        }

        public async Task<List<Customer>> GetCustomersFromSpecificCodeAsync(string specificCode)
        {
            return await _spaDbContext.Customers
                .Where(c => c.CustomerCode.CompareTo(specificCode) >= 0)
                .OrderBy(c => c.CustomerCode)
                .ToListAsync();
        }

        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        {
            return await _spaDbContext.Customers
                .Where(c => (c.LastName +" "+ c.FirstName).Contains(searchTerm) || (c.LastName + c.FirstName).Contains(searchTerm)
                || c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm) || c.Phone.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<string> UploadImageCustomer(CustomerPhoto customerPhoto)
        {
            await _spaDbContext.CustomerPhotos.AddAsync(customerPhoto);
            await _spaDbContext.SaveChangesAsync();
            return customerPhoto.PhotoPath;
        }


        public async Task<List<Appointment>> GetHistoryCustomer(long id)
        {
            List<Appointment> a = await _spaDbContext.Appointments.OrderByDescending(d => d.AppointmentDate)
                                                                    .Where(c => c.CustomerID == id)    // thiếu điều kiện chỉ khi hoàn thành và đã khám mới hiện lịch sử
                                                                    .Include(c => c.ChooseServices)
                                                                    .ThenInclude(s => s.Service).Include(p => p.CustomerPhotos)
                                                                    .ToListAsync();
            return a;
        }
    }
}

