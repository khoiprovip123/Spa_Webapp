using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreateServiceCommand : IRequest<long>
    {
        public ServiceDTO serviceDTO { get; set; }
    }

    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, long>
    {
        private readonly IServiceService _serviceService;

        public CreateServiceCommandHandler(IServiceService customerService)
        {
            _serviceService = customerService;
        }
        public async Task<long> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var serviceNew = new ServiceEntity
            {

                ServiceName = request.serviceDTO.ServiceName,
                ServiceCode = request.serviceDTO.ServiceCode,
                Description = request.serviceDTO.Description,
                Price = request.serviceDTO.Price
            };
            await _serviceService.CreateService(serviceNew);
            return serviceNew.ServiceID;
        }
    }


}
