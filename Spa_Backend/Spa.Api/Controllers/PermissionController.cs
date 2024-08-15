using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.Service;
using Spa.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IPermissionService _perService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public PermissionController(IPermissionService perService, IMapper mapper, IMediator mediator, IWebHostEnvironment env)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            _perService = perService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
        }

        [HttpGet("allPermissons")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var allPers = await _perService.GetAllPermissions();
            return Ok(allPers);
        }

        [HttpGet("GetPermissionsByJobType")]
        public async Task<ActionResult<List<Permission>>> GetPermissionsByJobType(long? jobTypeId)
        {
            try
            {
                var permissions = await _perService.GetAllPermissionByJobTypeID(jobTypeId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving permissions: {ex.Message}");
            }
        }

        [HttpGet("GetPermissionsByName")]
        public async Task<IActionResult> GetPermissionsByName(string perName)
        {
            try
            {
                var permission = await _perService.GetPermissionByName(perName);
                return Ok(permission);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving permissions: {ex.Message}");
            }
        }

        [HttpGet("GetPermissionNameByJobType")]
        public async Task<ActionResult<List<String>>> GetPermissionNameByJobType(long? jobTypeId)
        {
            try
            {
                var permissions = await _perService.GetAllPermissionNameByJobTypeID(jobTypeId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving permissions: {ex.Message}");
            }
        }

        [HttpPost("getRolePermissionByID")]
        public async Task<IActionResult> GetRolePermissionByID([FromBody] RolePermissionDTO rolePerDTO)
        {
            try
            {
                var rolePer = new RolePermissionDTO
                {
                    JobTypeID = rolePerDTO.JobTypeID,
                    PermissionID=rolePerDTO.PermissionID,
                };
                var newRolePer = await _perService.GetRolePermissionByID(rolePer.JobTypeID, rolePer.PermissionID);
                return Ok(new { newRolePer});
            }
            catch (Exception ex)
            {
                return Ok(new { });
            }
        }

        [HttpPost("createRolePermission")]
        public async Task<IActionResult> CreateRolePermission([FromBody] RolePermissionDTO rolePerDto)
        {
            try
            {
                var rolePer = new RolePermission
                {
                    JobTypeID=rolePerDto.JobTypeID,
                    PermissionID=rolePerDto.PermissionID,
                };
                var newRolePer = await _perService.CreateRolePermission(rolePer);
                return new JsonResult(newRolePer, _jsonSerializerOptions);
                //return Ok(new { newRolePer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("createPermission")]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDTO perDto)
        {
            try
            {
                var per = new Permission
                {
                    PermissionName = perDto.PermissionName,
                };
                var newPer = await _perService.CreatePermission(per);
                return Ok(new { newPer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("deleteRolePermission")]
        public async Task<ActionResult> DeleteRolePermission(RolePermissionDTO rolePerDTO)
        {
            try
            {
                bool delPer = await _perService.DeleteRolePermission(rolePerDTO.JobTypeID,rolePerDTO.PermissionID);
                return Ok(delPer);
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

        [HttpDelete("deletePermission")]
        public async Task<ActionResult> DeletePermission(long perID)
        {
            try
            {
                bool delPer = await _perService.DeletePermission(perID);
                return Ok(delPer);
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

        [HttpPut("updatePermission")]
        public async Task<IActionResult> UpdatePermission(long perID, [FromBody] PermissionDTO updateDto)
        {
            try
            {
                if (updateDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Permission per = new Permission
                {
                    PermissionID = perID,
                    PermissionName = updateDto.PermissionName,
                };
                await _perService.UpdatePermission(per);
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
    }
}
