using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public JobController(IJobService jobService, IMapper mapper, IMediator mediator, IWebHostEnvironment env)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jobService = jobService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
        }

        [HttpGet("allJobs")]
        [HasPermission(SetPermission.GetAllJobs)]
        public async Task<IActionResult> GetAllJobs()
        {
            var allJobs = await _jobService.GetAllJobs();
            return Ok(allJobs);
        }

        [HttpGet("allJobForPermissions")]
        //[HasPermission(SetPermission.GetAllJobs)]
        public async Task<IActionResult> GetAllJobForPermissions()
        {
            var allJobs = await _jobService.GetAllJobForPermissions();
            return Ok(allJobs);
        }

        [HttpGet("getJobTypeNameByID")]
        [HasPermission(SetPermission.GetJobTypeByID)]
        public async Task<IActionResult> GetJobTypeByID(long id)
        {
            try
            {
                var getJobTypeNameByID = _jobService.GetJobTypeByID(id);
                if (getJobTypeNameByID.Result is null)
                {
                    throw new Exception("Not Found!");
                }
                JobDTO jobDTO = new JobDTO
                {
                    JobTypeID = getJobTypeNameByID.Result.JobTypeID,
                    JobTypeName = getJobTypeNameByID.Result.JobTypeName,
                };
                return Ok(new { jobDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("createJob")]
        [HasPermission(SetPermission.CreateJobType)]
        public async Task<IActionResult> CreateJobType([FromBody] JobDTO jobDto)
        {
            try
            {
                var job = new JobType
                {
                    JobTypeName = jobDto.JobTypeName,
                };
                var newjob = await _jobService.CreateJobType(job);
                return Ok(new { id = newjob });
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

        [HttpPut("updateJob")]
        [HasPermission(SetPermission.UpdateJob)]
        public async Task<IActionResult> UpdateJob(long id, [FromBody] JobDTO updateDto)
        {
            try
            {
                if (updateDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                JobType job = new JobType
                {
                    JobTypeID = id,
                    JobTypeName = updateDto.JobTypeName,
                };
                await _jobService.UpdateJob(job);
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

        [HttpDelete("deleteJob")]
        [HasPermission(SetPermission.DeleteJob)]
        public async Task<ActionResult> DeleteJob(long id)
        {
            try
            {
                await _jobService.DeleteJob(id);
                return Ok(true);
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
