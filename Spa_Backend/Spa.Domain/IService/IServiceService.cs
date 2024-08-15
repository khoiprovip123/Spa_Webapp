using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IServiceService
    {
        Task<string> GenerateServiceCodeAsync();  //generate Id Customer

        IEnumerable<ServiceEntity> GetAllService(); //Get All

        Task CreateService(ServiceEntity serviceEntity); //create

        Task UpdateService(long serviceId, ServiceEntity serviceEntity); //update

        Task DeleteService(long ServiceId);

         ServiceEntity GetServiceById(long id); //get by id

        bool isExistService(long id); // check customer exist by Id

        Task<List<ServiceEntity>> SearchServicesAsync(string searchTerm);
    }
}
