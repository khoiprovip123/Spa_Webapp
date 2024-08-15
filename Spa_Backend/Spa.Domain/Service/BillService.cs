using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class BillService : IBillService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IBillRepository _billRepository;

        public BillService(IBillRepository billRepository, IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _billRepository = billRepository;
        }

        public async Task<bool> AddBillItem(List<BillItem> billItems)
        {
            return await _billRepository.AddBillItem(billItems); ;
        }

        public async Task<Bill> CreateBill(Bill bill)
        {

            return await _billRepository.CreateBill(bill);
        }

        public async Task<Bill?> GetBillByIdAsync(long id)
        {
            return await _billRepository.GetBillByIdAsync(id);
        }

        public async Task<Bill> GetNewBillAsync()
        {
            return await _billRepository.GetNewBillAsync();
        }

        public async Task<IEnumerable<Bill>> GetAllBillAsync()
        {
            return await _billRepository.GetAllBillAsync();
        }

        public async Task<Bill> UpdateBill(long id, Bill bill)
        {
            var billToUpdate = await _billRepository.GetBillByIdAsync(id);

            if (billToUpdate != null)
            {
                billToUpdate.BillStatus = bill.BillStatus;
                billToUpdate.TotalAmount = bill.TotalAmount;
                if (billToUpdate.BillItems != null)
                {
                    foreach (var item in billToUpdate.BillItems)
                    {
                       
                        item.Note = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.Note).FirstOrDefault() ?? null;
                        item.AmountDiscount = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.AmountDiscount).FirstOrDefault() ?? 0;
                        item.KindofDiscount = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.KindofDiscount).FirstOrDefault() ?? null;
                        item.Quantity = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.Quantity).FirstOrDefault();
                        item.UnitPrice = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.UnitPrice).FirstOrDefault();
                        item.TotalPrice = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.TotalPrice).FirstOrDefault();
                    }
                }
                await _billRepository.UpdateBill(billToUpdate);
            }

            return billToUpdate!;
        }

        public async Task<IEnumerable<Bill>> GetAllBillByCustomerAsync(long idCus)
        {
         return  await _billRepository.GetAllBillByCustomerAsync(idCus);
        }

        public async Task<IEnumerable<Object>> GetRevenueReport(long idBrand, DateTime fromDate, DateTime toDate)
        {
            return await _billRepository.GetRevenueReport(idBrand, fromDate, toDate);
        }

        public async Task<IEnumerable<object>> GetRevenueReportByDay(long idBrand, DateTime fromDate, DateTime toDate)
        {
           return await _billRepository.GetRevenueReportByDay(idBrand, fromDate, toDate);   
        }

        public async Task<IEnumerable<Bill>> GetBillByCustomer(long idCustomer)
        {
          var billList = await _billRepository.GetBillByCustomer(idCustomer);

            return billList;
        }

        public async Task<Bill> GetBillByAppointmentID(long id)
        {
            var bill = await _billRepository.GetBillByAppointmentID(id);
            return bill;
        }

        public async Task<string> GetLastCodeAsync()
        {
            return await _billRepository.GetLastCodeAsync();
        }
    }
}
