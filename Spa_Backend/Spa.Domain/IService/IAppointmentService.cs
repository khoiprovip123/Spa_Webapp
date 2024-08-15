using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IAppointmentService
    {
       Task CreateAppointmentAsync(Appointment appointment);

       IEnumerable<Appointment> GetAllAppoinment();

        Appointment GetAppointmentByIdAsync(long id);

        Task<bool> AddChooseServiceToappointment(long idApp, long idSer);

        Task<bool> AddAssignment(long idApp, long idEm);


        Task<Appointment> GetIdNewAppointment();

        Task<bool> DeleteAppointment(long idApp);

        Task<bool> UpdateAppointmentWithoutService(long id, Appointment appointment);

        Task<bool> UpdateAppointmentWithService(long id,List<long> serviceID, string status, string? notes);

        Task<bool> UpdateStatus(long id, string status);

        Task<bool> UpdateDiscount(long id, int perDiscount);

        Task<bool> AssignTechnicalStaff(long idApp, long idEm);

        Task<bool> UpdateAppointment(long idApp, Appointment appointment);

        Task<Object> GetAppointmentFromDayToDay(long branchID, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize);

        Task<Appointment> GetDetailAppointmentToCreateBill(long appointmentID);

        Task<List<Appointment>> SearchAppointment(DateTime fromDate, DateTime toDate, long branchId, string searchItem, int limit, int offset, string? status);

        Task<Object> GetAppointmentByStatus(long branchID, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize, string status);

        Task<int> CounterItemsAppointment(long branchID);
    }
}
