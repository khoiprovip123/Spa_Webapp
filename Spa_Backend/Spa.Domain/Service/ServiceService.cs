using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        public async Task CreateService(ServiceEntity serviceEntity)
        {
            var lastSerID = await GenerateServiceCodeAsync();
            if (await _serviceRepository.CheckExistNameCreateService(serviceEntity.ServiceName))
            {
                throw new DuplicateException("The name of service already exists in the system.");
            }
            serviceEntity.ServiceCode = lastSerID;
            _serviceRepository.CreateServiceEntitys(serviceEntity);
        }

        public async Task DeleteService(long serviceId)
        {
            var cusToDelete = _serviceRepository.GetServiceEntityById(serviceId);
            try
            {
                await _serviceRepository.DeleteServiceEntity(cusToDelete);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                throw new ForeignKeyViolationException("Cannot delete customer because it is referenced by other entities.");
            }
        }

        public async Task<List<ServiceEntity>> SearchServicesAsync(string searchTerm)
        {
            return await _serviceRepository.SearchServicesAsync(searchTerm);
        }

        public async Task<string> GenerateServiceCodeAsync()
        {
            var lastServiceCode = await _serviceRepository.GetLastServiceEntityAsync();

            if (lastServiceCode == null)
            {
                return "DV0001";
            }
            var lastCode = lastServiceCode.ServiceCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "DV" + numericPart.ToString("D4");
        }

        public IEnumerable<ServiceEntity> GetAllService()
        {
            return _serviceRepository.GetAllServiceEntity();
        }

        public ServiceEntity GetServiceById(long id)
        {
            return _serviceRepository.GetServiceEntityById(id);
        }

        public bool isExistService(long id)
        {
            return _serviceRepository.GetServiceEntityById(id) == null ? false : true;
        }



        public async Task UpdateService(long serviceId, ServiceEntity serviceEntity)
        {
            try
            {
                var serviceFromId = _serviceRepository.GetServiceEntityById(serviceId);
                bool checkName = await _serviceRepository.GetServiceEntityByName(serviceEntity.ServiceName, serviceId);
                if (checkName)
                {
                    throw new DuplicateException("The name of service already exists in the system.");
                }

                serviceFromId.ServiceName = serviceEntity.ServiceName;
                serviceFromId.Price = serviceEntity.Price;
                serviceFromId.Description = serviceEntity.Description;
                await _serviceRepository.UpdateServiceEntity(serviceFromId);
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
    }
}
