using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;

namespace Spa.Application.Commands
{
    public class CreateCustomerTypeCommand : IRequest<long>
    {
        public CustomerTypeDTO customerTypeDTO { get; set; }
    }

    public class CreateCustomerTypeCommandHandler : IRequestHandler<CreateCustomerTypeCommand, long>
    {
        private readonly ICustomerTypeService _customerTypeService;

        public CreateCustomerTypeCommandHandler(ICustomerTypeService customerTypeService)
        {
            _customerTypeService = customerTypeService;
        }
        public async Task<long> Handle(CreateCustomerTypeCommand request, CancellationToken cancellationToken)
        {
            var cusTypeNew = new CustomerType
            {

                CustomerTypeName = request.customerTypeDTO.CustomerTypeName,
            };
            await _customerTypeService.CreateCustomerType(cusTypeNew);
            return cusTypeNew.CustomerTypeID;
        }
    }


}
