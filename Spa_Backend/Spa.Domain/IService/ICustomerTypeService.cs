using Spa.Domain.Entities;

namespace Spa.Domain.IService
{
    public interface ICustomerTypeService
    {

        IEnumerable<CustomerType> GetAllCustomerTypes();

        Task CreateCustomerType(CustomerType customerType);

        Task UpdateCustomerType(CustomerType customerType);

        Task DeleteCustomerType(int customerId);

        Task<CustomerType> GetCustomerTypeById(int id);

        Task<bool> isExistCustomerType(int id);

        Task<bool> GetCustomerTypeByName(string name);

    }
}
