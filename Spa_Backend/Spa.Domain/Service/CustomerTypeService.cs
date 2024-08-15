using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;

namespace Spa.Domain.Service
{
    public class CustomerTypeService : ICustomerTypeService
    {
        private readonly ICustomerTypeRepository _customerTypeRepository;

        public CustomerTypeService(ICustomerTypeRepository customerTypeRepository)
        {
            _customerTypeRepository = customerTypeRepository;
        }

        public IEnumerable<CustomerType> GetAllCustomerTypes()
        {
            var customerTypes = _customerTypeRepository.GetAllCustomerTypes();
            return customerTypes;
        }


        public async Task CreateCustomerType(CustomerType customerType)
        {
            if (await _customerTypeRepository.CheckNameToCreateCustomerType(customerType.CustomerTypeName) != null)
            {
                throw new DuplicateException("The type already exists in the system.");
            }
            _customerTypeRepository.CreateCustomerType(customerType);
        }

        public async Task UpdateCustomerType(CustomerType customerType)
        {
            try
            {
                var customerTypeFromId = await _customerTypeRepository.GetCustomerTypeById(customerType.CustomerTypeID);
                if (customerTypeFromId is null)
                {
                    throw new Exception("Customer Type not exist!");
                }
                bool checkName = await GetCustomerTypeByName(customerType.CustomerTypeName);
                if (checkName)
                {
                    throw new DuplicateException("The type already exists in the system.");
                }
                await _customerTypeRepository.UpdateCustomerType(customerType);
            }
            catch (DuplicateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> GetCustomerTypeByName(string name)
        {
            bool flag = true;
            var customerType = await _customerTypeRepository.GetCustomerTypeByName(name);
            if (customerType == null)
            {
                flag = false;
            }
            return flag;
        }

        public async Task<CustomerType> GetCustomerTypeById(int id)
        {
            var cusType= await _customerTypeRepository.GetCustomerTypeById(id);
            return cusType;
        }

        public async Task DeleteCustomerType(int customerTypeId)
        {
            var cusTypeDelete = await GetCustomerTypeById(customerTypeId);
            try
            {
                await _customerTypeRepository.DeleteCustomerType(cusTypeDelete);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error!");
            }
        }

        public async Task<bool> isExistCustomerType(int id)
        {
            return await _customerTypeRepository.GetCustomerTypeById(id) == null ? false : true;
        }
    }
}
