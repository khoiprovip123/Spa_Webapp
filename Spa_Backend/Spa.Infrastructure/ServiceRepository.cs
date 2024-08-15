using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class ServiceRepository : EfRepository<ServiceEntity>, IServiceRepository
    {
        public ServiceRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public ServiceEntity CreateServiceEntitys(ServiceEntity serviceEntity)
        {
            Add(serviceEntity);
            return serviceEntity;
        }

        public async Task<bool> DeleteServiceEntity(ServiceEntity serviceEntity)
        {
            if (serviceEntity != null)
            {
                // customer.IsActive = false;
                // Update(customer);
                DeleteById(serviceEntity);
                return true;
            }
            return false;
        }

        public async Task<List<ServiceEntity>> SearchServicesAsync(string searchTerm)
        {
            return await _spaDbContext.Services
                .Where(s => (s.ServiceName).Contains(searchTerm))
                .ToListAsync();
        }

        public IEnumerable<ServiceEntity> GetAllServiceEntity()
        {
            return GetAll();
        }

        public async Task<ServiceEntity> GetLastServiceEntityAsync()
        {
            try
            {
                return await _spaDbContext.Services.OrderByDescending(s => s.ServiceCode).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> GetServiceEntityByName(string nameService, long id)
        {
            try
            {
                var service = await _spaDbContext.Services.FirstOrDefaultAsync(s => s.ServiceName == nameService && s.ServiceID != id);
                if (service != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

        public async Task<bool> CheckExistNameCreateService(string nameService)
        {
            try
            {
                var service = await _spaDbContext.Services.FirstOrDefaultAsync(s => s.ServiceName == nameService);
                if (service != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public ServiceEntity GetServiceEntityById(long idService)
        {
            return GetById(idService);
        }

        public Task<ServiceEntity> GetServiceEntityByPhone(string phone)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateServiceEntity(ServiceEntity ServiceEntity)
        {
            Update(ServiceEntity);
            return true;
        }

        public async Task<double> GetPriceService(long serviceID)
        {
            return await _spaDbContext.Services.Where(item => item.ServiceID == serviceID).Select(p => p.Price).FirstOrDefaultAsync();
        }

        
    }
}
