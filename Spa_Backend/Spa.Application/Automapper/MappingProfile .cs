using AutoMapper;
using Spa.Application.Models;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Automapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDTO>();
            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<ChooseService, ChooseServiceDTO>();
            CreateMap<ServiceEntity, ServiceDTO>();
            CreateMap<Assignment, AssignmentDTO>();
            CreateMap<ChooseServiceTreatment, ChooseServiceTreatmentDTO>();
            CreateMap<TreatmentDetail, TreatmentDetailDTO>();

        }
    }
}
