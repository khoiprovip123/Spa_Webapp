using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;

namespace Spa.Infrastructure
{
    public class CustomerTypeRepository : EfRepository<CustomerType>, ICustomerTypeRepository
    {
        public CustomerTypeRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public IEnumerable<CustomerType> GetAllCustomerTypes()
        {
            return GetAll();
        }

        public async Task<CustomerType> GetCustomerTypeById(int id)
        {
            var cusType = await _spaDbContext.ClientTypes.FirstOrDefaultAsync(c => c.CustomerTypeID == id);
            return cusType;
        }

        public async Task<CustomerType> GetCustomerTypeByName(string name)
        {
            return await _spaDbContext.ClientTypes.FirstOrDefaultAsync(c => c.CustomerTypeName == name);
        }

        public CustomerType CreateCustomerType(CustomerType customerType)
        {
            Add(customerType);
            return customerType;
        }

        public async Task<CustomerType> CheckNameToCreateCustomerType(string name)
        {
            return await _spaDbContext.ClientTypes.FirstOrDefaultAsync(c => c.CustomerTypeName == name);
        }

        public async Task<bool> UpdateCustomerType(CustomerType customerType)
        {
            var newUpdate = new CustomerType
            {
                CustomerTypeID = customerType.CustomerTypeID,
                CustomerTypeName = customerType.CustomerTypeName,
            };
            var cusTypeUpdate = await _spaDbContext.ClientTypes.FirstOrDefaultAsync(c => c.CustomerTypeID == newUpdate.CustomerTypeID);
            if (cusTypeUpdate is null) return false;
            {
                cusTypeUpdate.CustomerTypeName = newUpdate.CustomerTypeName;
            }
            _spaDbContext.ClientTypes.Update(cusTypeUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteCustomerType(CustomerType customerType)
        {
            if (customerType != null)
            {
                _spaDbContext.ClientTypes.Remove(customerType);
                _spaDbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
