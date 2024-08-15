using Spa.Domain.Entities;

namespace Spa.Domain.IRepository
{
    public interface ICustomerTypeRepository
    {
        CustomerType CreateCustomerType(CustomerType customertType);

        IEnumerable<CustomerType> GetAllCustomerTypes();

        Task<bool> UpdateCustomerType(CustomerType customertType);

        Task<bool> DeleteCustomerType(CustomerType customertType);

        Task<CustomerType> GetCustomerTypeById(int id);

        Task<CustomerType> GetCustomerTypeByName(string name);

        Task<CustomerType> CheckNameToCreateCustomerType(string name);
    }
}
