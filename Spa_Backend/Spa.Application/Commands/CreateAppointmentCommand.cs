using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreateAppointmentCommand : IRequest<long>
    {
        public long AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public long BranchID { get; set; }
        public long CustomerID { get; set; }
        //  public long EmployeeID { get; set; }
        public string? Status { get; set; }
        public double? Total { get; set; }

        public List<long>? ServiceID { get; set; }

        public List<long>? EmployeeID { get; set; }
        //public ICollection<ChooseServiceDTO>? ChooseServicesDTO { get; set; }
    }

    public class CreateAppontmentCommandHandler : IRequestHandler<CreateAppointmentCommand, long>
    {
        private readonly IAppointmentService _appointmentService;

        public CreateAppontmentCommandHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<long> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            Appointment app = new Appointment
            {
                AppointmentDate = request.AppointmentDate,
                CustomerID = request.CustomerID,
                BranchID = request.BranchID ,
                Status = request.Status,
                Total = request.Total,
                
            };
            await _appointmentService.CreateAppointmentAsync(app);
            var newAppointment = await _appointmentService.GetIdNewAppointment();



            CreateAppointmentDTO employees = new CreateAppointmentDTO
            {
                EmployeeID = request.EmployeeID,
            };

            foreach (var i in employees.EmployeeID)
            {
               await _appointmentService.AddAssignment(newAppointment.AppointmentID, i);
            }
            //if (request.ServiceID != null)
            //{              
            //    CreateAppointmentDTO service = new CreateAppointmentDTO
            //    {
            //        ServiceID = request.ServiceID,
            //    };             
            //    foreach (var i in service.ServiceID)
            //    {
            //        _appointmentService.AddChooseServiceToappointment(newAppointment.AppointmentID, i);
            //    }              
            //}

            return app.AppointmentID;

        }
    }
}
