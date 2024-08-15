using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ITreatmentRepository _treatmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IServiceRepository serviceRepository, ITreatmentRepository treatmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
            _treatmentRepository = treatmentRepository;
        }
        public async Task CreateAppointmentAsync(Appointment appointment)
        {
            _appointmentRepository.CreateAppointment(appointment);
        }

        public IEnumerable<Appointment> GetAllAppoinment()
        {
            return _appointmentRepository.GetAllAppointment();
        }

        public Appointment GetAppointmentByIdAsync(long id)
        {
            return _appointmentRepository.GetAppointmentByID(id);
        }

        public async Task<Appointment> GetIdNewAppointment()
        {
            return await _appointmentRepository.GetNewAppoinmentAsync();

        }

        public async Task<bool> AddChooseServiceToappointment(long idApp, long idSer)
        {
            return await _appointmentRepository.AddChooseService(idApp, idSer);
        }

        public async Task<bool> DeleteAppointment(long idApp)
        {
            var checkAppointment = _appointmentRepository.GetAppointmentByID(idApp);
            if (checkAppointment != null)
            {
                if (checkAppointment.Status.Equals("Completed"))
                {
                    throw new ErrorMessage("Can not delete when appointment is completed");
                }
                _appointmentRepository.DeleteAppointment(checkAppointment);
                return true;
            }
            return false;
        }



        public Task<bool> AddAssignment(long idApp, long idEm)
        {
            return _appointmentRepository.AddAssignment(idApp, idEm);
        }

        public async Task<bool> UpdateAppointmentWithoutService(long id, Appointment appointment)
        {
            var appointmentToUpdate = _appointmentRepository.GetAppointmentByID(id);

            var currentAssign = appointment.Assignments.ToList();

            var currentEmployees = appointmentToUpdate.Assignments.Select(e => e.EmployerID).ToList();

            var employeeToRemove = currentEmployees.Except(currentAssign.Select(e => e.EmployerID));

            if (appointment.AppointmentDate != null)
            {
                appointmentToUpdate.AppointmentDate = appointment.AppointmentDate;
            }
            if (appointment.Status != null)
            {
                appointmentToUpdate.Status = appointment.Status;
            }

            foreach (var employee in employeeToRemove)
            {
                var assign = appointmentToUpdate.Assignments.FirstOrDefault(e => e.EmployerID == employee);

                if (assign != null)
                {
                    appointmentToUpdate.Assignments.Remove(assign);
                }
            }

            var employeeToAdd = currentAssign.Select(e => e.EmployerID).Except(currentEmployees).ToList();
            foreach (var employeeId in employeeToAdd)
            {
                appointmentToUpdate.Assignments.Add(new Assignment
                {
                    AppointmentID = id,
                    EmployerID = employeeId
                });
            }

            return await _appointmentRepository.UpdateAppointmentWithoutService(appointmentToUpdate);
        }

        public async Task<bool> UpdateAppointmentWithService(long id, List<long> serviceIDs, string? status, string? notes)
        {
            var getChooseServiceByAppointment = await _appointmentRepository.ListService(id);

            if (getChooseServiceByAppointment == null)
            {
                await _appointmentRepository.updateServiceInAppointmentByDoctor(id, serviceIDs);
            }
            else
            {
                var currentServiceIDs = getChooseServiceByAppointment.Select(se => se.ServiceID).ToList();

                var serviceToRemove = currentServiceIDs.Except(serviceIDs).ToList();
                if (serviceToRemove.Count > 0)
                {
                    await _appointmentRepository.RemoveChooseService(id, serviceToRemove);
                }

                var serviceToAdd = serviceIDs.Except(currentServiceIDs).ToList();
                if (serviceToAdd.Count > 0)
                {
                    await _appointmentRepository.updateServiceInAppointmentByDoctor(id, serviceToAdd);
                }
            }
            var app = GetAppointmentByIdAsync(id);
            var listPrice = await _appointmentRepository.GetAllPriceService(id);
            app.Total = 0;
            app.Status = status;
            foreach (var price in listPrice)
            {
                app.Total += price;
            }

            if (notes != null)
            {
                app.Notes = notes;
            }

            _appointmentRepository.UpdateTotalAppointment(app);
            return true;
        }

        public async Task<bool> UpdateStatus(long id, string status)
        {
            var appointmentToUpdate = _appointmentRepository.GetAppointmentByID(id);
            appointmentToUpdate.Status = status;
            await _appointmentRepository.UpdateAppointmentWithoutService(appointmentToUpdate);
            return true;
        }

        public async Task<bool> UpdateDiscount(long id, int perDiscount)
        {
            var app = _appointmentRepository.GetAppointmentByID(id);

            if (perDiscount != null)
            {
                app.DiscountPercentage = perDiscount;
                app.DiscountAmount = app.Total * ((double)perDiscount / 100);
                _appointmentRepository.UpdateAppointment(app);
            }
            return true;
        }

        public async Task<bool> AssignTechnicalStaff(long idApp, long idEm)
        {
            //Jobid 2 là bác sĩ
            var appToUpdate = GetAppointmentByIdAsync(idApp);
            if (appToUpdate.Assignments.FirstOrDefault(e => e.Employees.JobTypeID == 3) != null)
            {
                if (!appToUpdate.Assignments.Where(em => em.EmployerID == idEm).IsNullOrEmpty())
                {
                    return true;
                }
                else
                {
                    var oldStaff = appToUpdate.Assignments.Where(e => e.Employees.JobTypeID == 3).FirstOrDefault();
                    appToUpdate.Assignments.Remove(oldStaff);
                    appToUpdate.Assignments.Add(new Assignment { AppointmentID = idEm, EmployerID = idEm });
                    _appointmentRepository.UpdateAppointment(appToUpdate);
                    return true;
                }
            }
            else
            {
                appToUpdate.Assignments.Add(new Assignment { AppointmentID = idEm, EmployerID = idEm });
                _appointmentRepository.UpdateAppointment(appToUpdate);
                return true;
            }
        }

        public async Task<bool> UpdateAppointment(long idApp, Appointment appointment)
        {
            try
            {
                var appToUpdate = await _appointmentRepository.GetAppointmentByIdAsync(idApp);
                UpdateNonNullFields(appToUpdate, appointment);
                var chooseService = appToUpdate.ChooseServices;
                if (appointment.ChooseServiceTreatments != null)
                {
                    appToUpdate.ChooseServiceTreatments = appointment.ChooseServiceTreatments;
                }
                foreach (var item in appointment.ChooseServiceTreatments)
                {
                    var treatmentdetail = await _treatmentRepository.GetTreatmentDetailAsyncByID(item.TreatmentDetailID);
                    treatmentdetail.QuantityDone++;
                    if (treatmentdetail.QuantityDone == treatmentdetail.Quantity)
                    {
                        treatmentdetail.IsDone = true;
                    }
                }
                appToUpdate.Total = 0;
                foreach (var item in chooseService)
                {
                    appToUpdate.Total += await _serviceRepository.GetPriceService(item.ServiceID);
                }
                return await _appointmentRepository.UpdateAppointmentAsync(appToUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateNonNullFields(Appointment target, Appointment source)
        {
            var properties = typeof(Appointment).GetProperties();
            target.ChooseServices.Clear();

            foreach (var property in properties)
            {
                var sourceValue = property.GetValue(source);
                if (sourceValue != null)
                {
                    property.SetValue(target, sourceValue);
                }
            }
        }

        public async Task<Object> GetAppointmentFromDayToDay(long branchID, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            return await _appointmentRepository.GetAppointmentFromDayToDay(branchID, fromDate, toDate, pageNumber, pageSize);
        }

        public async Task<Appointment> GetDetailAppointmentToCreateBill(long appointmentID)
        {
            var app = await _appointmentRepository.GetDetailAppointmentToCreateBill(appointmentID);
            return app;
        }

        public async Task<List<Appointment>> SearchAppointment(DateTime fromDate, DateTime toDate, long branchId, string searchItem, int limit, int offset, string? status)
        {
            return await _appointmentRepository.SearchAppointment(fromDate, toDate, branchId, searchItem, limit, offset, status);
        }

        public async Task<Object> GetAppointmentByStatus(long branchID, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize, string status)
        {
            return await _appointmentRepository.GetAppointmentByStatus(branchID, fromDate, toDate, pageNumber, pageSize, status);
        }

        public async Task<int> CounterItemsAppointment(long branchID)
        {
            return await _appointmentRepository.CounterItemsAppointment(branchID);
        }
    }
}
