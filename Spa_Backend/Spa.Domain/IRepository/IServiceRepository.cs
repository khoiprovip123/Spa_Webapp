
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IServiceRepository
    {
        ServiceEntity CreateServiceEntitys(ServiceEntity ServiceEntity);
    
        IEnumerable<ServiceEntity> GetAllServiceEntity();

        Task<ServiceEntity> GetLastServiceEntityAsync();

        Task<bool> UpdateServiceEntity(ServiceEntity ServiceEntity);

        Task<bool> DeleteServiceEntity(ServiceEntity ServiceEntity);

        ServiceEntity GetServiceEntityById(long id);


        Task<bool> GetServiceEntityByName(string nameService, long id);

        Task<bool> CheckExistNameCreateService(string nameService);

        Task<List<ServiceEntity>> SearchServicesAsync(string searchTerm);
        Task<double> GetPriceService(long serviceID);
    }
}
