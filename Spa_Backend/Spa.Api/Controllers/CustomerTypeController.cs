using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;


namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerTypeController : ControllerBase
    {

        private readonly ICustomerTypeService _customerTypeService;
        private readonly IMediator _mediator;

        public CustomerTypeController(ICustomerTypeService customerTypeService, IMediator mediator)
        {
            _customerTypeService = customerTypeService;
            _mediator = mediator;
        }

        [HttpGet("allCustomerType")]
        [HasPermission(SetPermission.GetAllCustomerTypes)]
        public ActionResult GetAll()
        {
            var allCusType = _customerTypeService.GetAllCustomerTypes();
            var cusTypeDTO = allCusType.Select(c => new CustomerTypeDTO
            {
                CustomerTypeID = c.CustomerTypeID,
                CustomerTypeName = c.CustomerTypeName,

            }).OrderBy(c => c.CustomerTypeID).ToList();
            return Ok(new { cusTypeDTO });
        }

        [HttpGet(("customerTypeById"))]
        [HasPermission(SetPermission.GetCustomerTypeById)]
        public async Task<IActionResult> GetCustomerTypeById(int id)
        {
            var getByCusTypeByID = _customerTypeService.GetCustomerTypeById(id);
            if (getByCusTypeByID.Result is null)
            {
                return Ok(new { });
            }
            CustomerTypeDTO cusTypeDTO = new CustomerTypeDTO
            {
                CustomerTypeID = getByCusTypeByID.Result.CustomerTypeID,
                CustomerTypeName = getByCusTypeByID.Result.CustomerTypeName,
            };
            return Ok(new { cusTypeDTO });
        }

        [HttpPost("createCustomerType")]
        [HasPermission(SetPermission.CreateCustomerType)]
        public async Task<IActionResult> CreateService([FromBody] CustomerTypeDTO customerTypeDto)
        {
            try
            {
                var command = new CreateCustomerTypeCommand
                {
                    customerTypeDTO = customerTypeDto
                };
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (DuplicateException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("updateCustomerType")]
        [HasPermission(SetPermission.UpdateCustomerType)]
        public async Task<IActionResult> UpdateCustomerType(int customerTypeId, [FromBody] CustomerTypeDTO customerTypeDto)
        {
            try
            {
                if (customerTypeDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                CustomerType cusType = new CustomerType
                {
                    CustomerTypeID = customerTypeId,
                    CustomerTypeName = customerTypeDto.CustomerTypeName,
                };
                await _customerTypeService.UpdateCustomerType(cusType);
                return Ok(true);
            }
            catch (DuplicateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("deleteCustomerType")]
        [HasPermission(SetPermission.DeleteCustomerType)]
        public async Task<ActionResult> DeleteCustomerType(int id)
        {
            try
            {
                if (await _customerTypeService.isExistCustomerType(id))
                {
                    await _customerTypeService.DeleteCustomerType(id);
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (ForeignKeyViolationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
