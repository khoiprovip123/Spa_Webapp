using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Newtonsoft.Json.Serialization;
using Spa.Api.Attributes;
using Spa.Application;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.InteropServices;
using Spa.Application.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IHubContext<ChatHub> _hubContext;

        public AppointmentController(IAppointmentService appointmentService, IMediator mediator, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _appointmentService = appointmentService;
            _mediator = mediator;
            _mapper = mapper;
            _hubContext = hubContext;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        [HttpGet]
        [HasPermission(SetPermission.GetAllApointment)]
        [Cache(1000)]
        public ActionResult GetAll(long idBrand)
        {
            var app = _appointmentService.GetAllAppoinment().Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
                EmployeeCode = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                SpaTherapist = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
            });

            var appByBrand = app.Where(e => e.BranchID == idBrand && e.AppointmentDate >= DateTime.Today);

            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(appByBrand, _jsonSerializerOptions);
        }

        [HttpGet("InfoToCreateBill")]
        [HasPermission(SetPermission.InfoToCreateBill)]
        public async Task<ActionResult> GetDetailAppointmentToCreateBill(long apointmentID)
        {
            var app = await _appointmentService.GetDetailAppointmentToCreateBill(apointmentID);
            List<Employee> employees = new List<Employee>();
            List<ServiceEntity> services = new List<ServiceEntity>();

            foreach (var item in app.ChooseServices)
            {
                services.Add(item.Service);
            }

            foreach (var item in app.Assignments)
            {
                employees.Add(item.Employees);
            }

            var infoToCreateBill = new
            {
                AppointmentID = app.AppointmentID,
                CustomerID = app.CustomerID,
                Employees = employees.Select(a => new
                {
                    EmployeeID = a.EmployeeID,
                    FirtName = a.FirstName,
                    LastName = a.LastName,
                    JobName = a.JobType.JobTypeName,
                }),
                Services = services.Select(s => new
                {
                    serviceCode = s.ServiceCode,
                    serviceName = s.ServiceName,
                    price = s.Price,
                })
            };
            return new JsonResult(infoToCreateBill, _jsonSerializerOptions);
        }

        [HttpGet("/GetAllByBranch")]
        [HasPermission(SetPermission.GetAllByBranch)]
        public ActionResult GetAllByBranch(long idBrand)
        {
            var app = _appointmentService.GetAllAppoinment().Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
                EmployeeCode = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                SpaTherapist = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
            });

            var appByBrand = app.Where(e => e.BranchID == idBrand && e.AppointmentDate >= DateTime.Today);

            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(appByBrand, _jsonSerializerOptions);
        }

        [HttpGet("/GetAppointmentByStatus")]
        [HasPermission(SetPermission.GetAllByStatus)]
        public ActionResult GetAllByStatus(long idBrand, string status)
        {
            var app = _appointmentService.GetAllAppoinment().Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                EmployeeCode = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
                SpaTherapist = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
            });

            var appByBrand = app.Where(e => e.BranchID == idBrand && e.Status == status && e.AppointmentDate >= DateTime.Today);

            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(appByBrand, _jsonSerializerOptions);
        }

        [HttpGet("getbyday")]
        //[HasPermission(SetPermission.GetAppointmentByDay)]
        public async Task<ActionResult> GetAppointmentByDay(long branchID, DateTime fromDate, DateTime toDate, int pageNumber = 1, int pageSize = 10)
        {
            var app = await _appointmentService.GetAppointmentFromDayToDay(branchID, fromDate, toDate, pageNumber, pageSize);
            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(app, _jsonSerializerOptions);
        }

        [HttpPost]
        [HasPermission(SetPermission.CreateAppointment)]
        public async Task<ActionResult> Add([FromBody] CreateAppointmentDTO appointmentCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new CreateAppointmentCommand
                {
                    CustomerID = appointmentCreateDto.CustomerID,
                    AppointmentDate = appointmentCreateDto.AppointmentDate,
                    BranchID = appointmentCreateDto.BranchID,
                    EmployeeID = appointmentCreateDto.EmployeeID,
                    Status = appointmentCreateDto.Status,
                    Total = appointmentCreateDto.Total,
                    ServiceID = appointmentCreateDto.ServiceID,
                };
                var id = await _mediator.Send(command);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
       // [HasPermission(SetPermission.GetAppointmentById)]
        public async Task<ActionResult> GetAppointmentById(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var getDoctorAndStaff = _appointmentService.GetAppointmentByIdAsync(id);
            var appByid = _mapper.Map<AppointmentDTO>(_appointmentService.GetAppointmentByIdAsync(id));
            AppointmentDTO appointmentDTO = new AppointmentDTO
            {
                TeachnicalStaff = getDoctorAndStaff.Assignments.Where(e => e.Employees.JobTypeID == 3)
                .Select(n => n.Employees.LastName + " " + n.Employees.FirstName).FirstOrDefault(),

                SpaTherapist = getDoctorAndStaff.Assignments.Where(e => e.Employees.JobTypeID == 3)
                .Select(n => n.Employees.EmployeeCode).FirstOrDefault(),

                Doctor = getDoctorAndStaff.Assignments.Where(e => e.Employees.JobTypeID == 2)
                .Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault()
            };
            appByid.Doctor = appointmentDTO.Doctor;
            appByid.TeachnicalStaff = appointmentDTO.TeachnicalStaff;
            appByid.SpaTherapist = appointmentDTO.SpaTherapist;

            if (appByid == null)
            {
                return NotFound();
            }
            return new JsonResult(appByid, _jsonSerializerOptions); ;
        }

        [HttpPut("updatestatus/{id}")]
        [HasPermission(SetPermission.UpdateStatus)]
        public async Task<ActionResult> updateStatus(long id, string status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _appointmentService.UpdateStatus(id, status);
                await _hubContext.Clients.All.SendAsync("ChangeStatus", "true");
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return Ok();
        }

        [HttpPut("assigntechnicalstaff")]
        [HasPermission(SetPermission.AssignTechnicalStaff)]
        public async Task<ActionResult> AssignTechnicalStaff(long idApp, long idEmploy)
        {
            await _appointmentService.AssignTechnicalStaff(idApp, idEmploy);
            return Ok(true);
        }


        [HttpPut("{id}")]
        [HasPermission(SetPermission.UpdateAppointmentWithoutService)]
        public async Task<ActionResult> updateAppointmentWithoutService(long id, [FromBody] UpdateAppointmentWithoutServiceDTO updateAppointmentWithoutServiceDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Appointment app = new Appointment
                {
                    AppointmentDate = updateAppointmentWithoutServiceDTO.AppointmentDate,
                    Status = updateAppointmentWithoutServiceDTO.Status,
                    Assignments = updateAppointmentWithoutServiceDTO.Assignments.Select(a => new Assignment
                    {
                        EmployerID = a.EmployerID,
                    }).ToList()
                };

                await _appointmentService.UpdateAppointmentWithoutService(id, app);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        [HttpPut("api/UpdateAppointmentWithService/{id}/{status}")]
        [HasPermission(SetPermission.UpdateAppointmentWithService)]
        public async Task<ActionResult> updateAppointmentWithService(long id, string status, [FromBody] List<long> serviceID, string? notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _appointmentService.UpdateAppointmentWithService(id, serviceID, status, notes);
            return Ok(new { id });
        }

        [HttpPut("Test/{id}")]
       // [HasPermission(SetPermission.UpdateAppointment)]
        public async Task<ActionResult> UpdateAppointment(long id, UpdateAppointmentDTO appointment)  //Update Appointment (RESTFUll)
        {
            ICollection<ChooseService>? chooseServices = new List<ChooseService>();
            ICollection<ChooseServiceTreatment>? choosetreatmentDetails = new List<ChooseServiceTreatment>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_appointmentService.GetAppointmentByIdAsync(id) != null)
            {
                if (appointment.ListServiceID != null)
                {
                    foreach (var item in appointment.ListServiceID)
                    {                      
                            chooseServices!.Add(new ChooseService { AppointmentID = id, ServiceID = item });                        
                    }
                }
                if (appointment.ChooseServiceTreatmentDTO != null)
                {
                   foreach(var item in appointment.ChooseServiceTreatmentDTO)
                    {
                        choosetreatmentDetails.Add(new ChooseServiceTreatment
                        {
                            TreatmentDetailID = item.TreatmentDetailID,
                            AppointmentID = id,
                            QualityChooses = item.QualityChooses,
                        });
                    }
                }


                Appointment app = new Appointment
                {
                    AppointmentID = id,
                    AppointmentDate = appointment.AppointmentDate,
                    BranchID = appointment.BranchID,
                    CustomerID = appointment.CustomerID,
                    Notes = appointment.Notes,
                    DiscountPercentage = appointment.DiscountPercentage,
                    Status = appointment.Status,
                    ChooseServices = chooseServices,
                    ChooseServiceTreatments = choosetreatmentDetails

                };

                await _appointmentService.UpdateAppointment(id, app);
                await _hubContext.Clients.All.SendAsync("ChangeStatus", "true");
                return Ok(new { id, appointment });
            }
            return NotFound();

        }

        [HttpDelete("{id}")]
        [HasPermission(SetPermission.DeleteAppointmentById)]
        public async Task<ActionResult> DeleteAppointmentById(long id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _appointmentService.DeleteAppointment(id))
                {
                    return Ok(new { id });
                }
            }
            catch (ErrorMessage ex)
            {
                return BadRequest(new { Message = ex.Message }); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return NotFound();
        }

        [HttpPut("UpdateDiscount")]
        [HasPermission(SetPermission.UpdateDiscount)]
        public async Task<ActionResult> UpdateDiscount(long id, int perDiscount)
        {
            var a = await _appointmentService.UpdateDiscount(id, perDiscount);
            return Ok(a);
        }

        [HttpGet("searchAppointment")]
        public async Task<ActionResult> SearchAppointment(DateTime fromDate, DateTime toDate, long branchId, string? searchItem, int limit, int offset, string? status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var app = await _appointmentService.SearchAppointment(fromDate, toDate, branchId, searchItem, limit, offset, status);
            var listApp = app.Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
                EmployeeCode = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                SpaTherapist = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.EmployeeCode).FirstOrDefault(),
            });
            var total = app.Count();
            var pageList = new
            {
                litmit = limit,
                offSet = offset,
                totalItems = total,
                items = listApp
            };

            return new JsonResult(pageList, _jsonSerializerOptions);
        }


        [HttpGet("GetByStatusWithPaging")]
        public async Task<ActionResult> GetAppointmentByStatus(long branchID, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize, string? status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var listByStatus = await _appointmentService.GetAppointmentByStatus(branchID, fromDate, toDate, pageNumber, pageSize, status);
               
                return new JsonResult(listByStatus, _jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        



    }
}
