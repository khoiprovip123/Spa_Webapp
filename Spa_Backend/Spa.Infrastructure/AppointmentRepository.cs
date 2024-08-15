using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class AppointmentRepository : EfRepository<Appointment>, IAppointmentRepository
    {

        public AppointmentRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public async Task<bool> AddChooseService(long idApp, long idSer)
        {
            ChooseService chooseService = new ChooseService
            {
                AppointmentID = idApp,
                ServiceID = idSer
            };
            await _spaDbContext.AddAsync(chooseService);
            await _spaDbContext.SaveChangesAsync();
            return true;
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            Add(appointment);
            return appointment;
        }

        public async Task<Appointment> GetDetailAppointmentToCreateBill(long appointmentID)
        {
            IQueryable<Appointment> query = _spaDbContext.Appointments.Include(a => a.Assignments!)
                .ThenInclude(a => a.Employees!).ThenInclude(j => j.JobType!)
                .Include(c => c.ChooseServices!)
                .ThenInclude(c => c.Service)
                .Where(a => a.AppointmentID == appointmentID);

            var app = await query.FirstOrDefaultAsync();

            return app;
        }

        public async Task<bool> UpdateAppointmentWithoutService(Appointment appointment)
        {
            _spaDbContext.UpdateRange(appointment);
            await _spaDbContext.SaveChangesAsync();
            return true;
        }

        public Assignment CreateAssignment(Assignment assignment)
        {
            _spaDbContext.Assignments.Add(assignment);
            return assignment;
        }

        public bool DeleteAppointment(Appointment appointment)   // không dùng
        {
            var idApp = appointment.AppointmentID;
            var listIdDelete = _spaDbContext.ChooseServices.Where(c => c.AppointmentID == appointment.AppointmentID).ToList();
            _spaDbContext.RemoveRange(listIdDelete);
            _spaDbContext.Appointments.Remove(appointment);
            _spaDbContext.SaveChanges();
            return true;
        }

        public IEnumerable<Appointment> GetAllAppointment()
        {
            IQueryable<Appointment> query = _spaDbContext.Appointments
                                .Include(c => c.Customer)
                                .Include(a => a.Assignments!).ThenInclude(e => e.Employees)
                                .Include(s => s.ChooseServices!).ThenInclude(se => se.Service);

            return query.ToList();
        }

        public Appointment GetAppointmentByID(long appointmentId)
        {
            IQueryable<Appointment> query = _spaDbContext.Appointments.Where(a => a.AppointmentID == appointmentId)
                                             .Include(c => c.Customer)
                                             .Include(e => e.Assignments!).ThenInclude(em => em.Employees)
                                             .Include(s => s.ChooseServices!).ThenInclude(se => se.Service)
                                             .Include(c => c.ChooseServiceTreatments).ThenInclude(de => de.TreatmentDetail);
            return query.FirstOrDefault()!;
        }



        public void UpdateAppointment(Appointment appointment)
        {
            Update(appointment);
        }

        public async Task<Appointment?> GetNewAppoinmentAsync()
        {
            try
            {
                IQueryable<Appointment> query = _spaDbContext.Appointments.OrderByDescending(c => c.AppointmentID);
                var app = await query.FirstOrDefaultAsync();
                if (app is not null)
                {
                    return app;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> updateServiceInAppointmentByDoctor(long id, List<long> serviceID)
        {
            foreach (var i in serviceID)
            {
                ChooseService chooseservice = new ChooseService
                {
                    AppointmentID = id,
                    ServiceID = i
                };
                await _spaDbContext.AddAsync(chooseservice);
                await _spaDbContext.SaveChangesAsync();
            }
            return true;
        }



        public async Task<bool> AddAssignment(long idApp, long idEm)
        {
            Assignment assignment = new Assignment
            {
                AppointmentID = idApp,
                EmployerID = idEm
            };
            await _spaDbContext.AddAsync(assignment);
            await _spaDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ChooseService>> ListService(long id)
        {
            IQueryable<ChooseService> query = _spaDbContext.ChooseServices.Where(a => a.AppointmentID == id);
            return await query.ToListAsync();
        }

        public async Task RemoveChooseService(long id, List<long> serviceIDs)
        {
            foreach (var serviceID in serviceIDs)
            {
                var chooseservice = await _spaDbContext.ChooseServices
                    .FirstOrDefaultAsync(cs => cs.AppointmentID == id && cs.ServiceID == serviceID);

                if (chooseservice != null)
                {
                    _spaDbContext.ChooseServices.Remove(chooseservice);
                }
            }
            await _spaDbContext.SaveChangesAsync();
        }

        public async Task<List<double>> GetAllPriceService(long idApp)
        {
            IQueryable<double> query = _spaDbContext.ChooseServices.Where(c => c.AppointmentID == idApp).Include(se => se.Service).Select(p => p.Service.Price);
            return await query.ToListAsync();
        }

        public bool UpdateTotalAppointment(Appointment appointment)
        {
            Update(appointment);
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            try
            {
                _spaDbContext.UpdateRange(appointment);
                await _spaDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(long idApp)
        {
            IQueryable<Appointment> query = _spaDbContext.Appointments.Where(a => a.AppointmentID == idApp)
                                            .Include(c => c.Customer)
                                            .Include(e => e.Assignments!).ThenInclude(em => em.Employees)
                                            .Include(s => s.ChooseServices!).ThenInclude(se => se.Service);

            var appToUpdate = await query.FirstOrDefaultAsync();
            return appToUpdate!;
        }

        public async Task<Object> GetAppointmentFromDayToDay(long branchID, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            IQueryable<Appointment> query = _spaDbContext.Appointments.OrderBy(d => d.AppointmentDate).Where(a => a.BranchID == branchID && a.AppointmentDate >= fromDate && a.AppointmentDate <= toDate)
                                .Include(c => c.Customer)
                                .Include(e => e.Assignments!).ThenInclude(em => em.Employees)
                                .Include(s => s.ChooseServices!).ThenInclude(se => se.Service);

            var totalItems = await query.CountAsync();

            var paging = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                                .ToListAsync();

            var items = paging.Select(a => new
            {
                appointmentID = a.AppointmentID,
                appointmentDate = a.AppointmentDate,
                branchID = a.BranchID,
                customerID = a.CustomerID,
                status = a.Status,
                customer = new Customer()
                {
                    FirstName = a.Customer.FirstName,
                    LastName = a.Customer.LastName,
                    CustomerCode = a.Customer.CustomerCode,
                    Phone = a.Customer.Phone,
                    DateOfBirth = a.Customer.DateOfBirth,
                },
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
            });


            var response = new
            {
                offset = pageNumber,
                limit = pageSize,
                totalItems = totalItems,
                items = items
            };
            return response;

        }

        public async Task<Object> GetAppointmentByStatus(long branchID, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize, string status)
        {
            IQueryable<Appointment> query = _spaDbContext.Appointments.Where(a => a.BranchID == branchID && a.AppointmentDate >= fromDate && a.AppointmentDate <= toDate && a.Status!.Equals(status))
                                .Include(c => c.Customer)
                                .Include(e => e.Assignments!).ThenInclude(em => em.Employees)
                                .Include(s => s.ChooseServices!).ThenInclude(se => se.Service);
            var countTotal = query.Count();

            var listApp = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var listAppDTO = listApp.Select(a => new
            {
                appointmentID = a.AppointmentID,
                appointmentDate = a.AppointmentDate,
                branchID = a.BranchID,
                customerID = a.CustomerID,
                status = a.Status,
                customer = new Customer()
                {
                    FirstName = a.Customer.FirstName,
                    LastName = a.Customer.LastName,
                    CustomerCode = a.Customer.CustomerCode,
                    Phone = a.Customer.Phone,
                    DateOfBirth = a.Customer.DateOfBirth,
                },
                Doctor = a.Assignments!.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.LastName + " " + e.Employees.FirstName).FirstOrDefault(),
            });

            var response = new
            {
                offset = pageNumber,
                limit = pageSize,
                totalItems = countTotal,
                items = listAppDTO
            };
            return response;
        }

        public async Task<List<Appointment>> SearchAppointment(DateTime fromDate, DateTime toDate, long branchId, string searchItem, int limit, int offset, string? status)
        {
            IQueryable<Appointment> searchList = _spaDbContext.Appointments.OrderBy(o => o.AppointmentDate)
                .Include(c => c.Customer)
                .Include(a => a.Assignments!).ThenInclude(e => e.Employees)
                .Include(s => s.ChooseServices!).ThenInclude(se => se.Service)
                .Where(a => a.AppointmentDate >= fromDate
                && a.AppointmentDate <= toDate
                && a.BranchID == branchId
                && (a.Customer.FirstName.Contains(searchItem)
                || a.Customer.LastName.Contains(searchItem)
                || a.Customer.Phone.Contains(searchItem)));

            if (!string.IsNullOrEmpty(status) && !status.Equals(""))
            {
                searchList = searchList.Where(a => a.Status.Equals(status));
            }

            var response = await searchList.ToListAsync();
            var paging = response.Skip((offset - 1) * limit)
                .Take(limit);
            return response;
        }

        public async Task<int> CounterItemsAppointment(long branchID)
        {
            IQueryable<Appointment> query = _spaDbContext.Appointments.Where(e => e.BranchID == branchID);
            var countTotal = await query.CountAsync();
            return countTotal;
        }
    }
}
