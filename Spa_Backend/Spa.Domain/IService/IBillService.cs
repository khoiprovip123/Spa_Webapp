using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IBillService
    {
        Task<Bill> CreateBill(Bill bill);

        Task<Bill> GetNewBillAsync();

        Task<bool> AddBillItem(List<BillItem> billItems);

        Task<Bill?> GetBillByIdAsync(long id);

        Task<IEnumerable<Bill>> GetAllBillAsync();

        Task<IEnumerable<Bill>> GetAllBillByCustomerAsync(long idCus);

        Task<Bill> UpdateBill(long id, Bill bill);

        Task<IEnumerable<Object>> GetRevenueReport(long idBrand, DateTime fromDate, DateTime toDate); //thognke

        Task<IEnumerable<Object>> GetRevenueReportByDay(long idBrand, DateTime fromDate, DateTime toDate);

        Task<IEnumerable<Bill>> GetBillByCustomer(long idCustomer);

        Task<Bill?>GetBillByAppointmentID(long appId);

        Task<string> GetLastCodeAsync();
    }
}
