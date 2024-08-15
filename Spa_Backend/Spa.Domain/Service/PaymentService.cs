using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IIncomeExpensesRepository _incomeExpensesRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IBillRepository _billRepository;

        public PaymentService(IPaymentRepository paymentRepository, IAppointmentRepository appointmentRepository, IBillRepository billRepository, IIncomeExpensesRepository incomeExpensesRepository)
        {
            _incomeExpensesRepository = incomeExpensesRepository;
            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _billRepository = billRepository;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            try
            {
                payment.PaymentDate = DateTime.Now;
                var paymentProcess = await _paymentRepository.AddPayment(payment);
                if (paymentProcess)
                {                                 
                    var bill = await _billRepository.GetBillByIdAsync(payment.BillID);
                    bill.AmountInvoiced += payment.Amount;
                    bill.AmountResidual -= payment.Amount;
                    if(bill.AmountResidual == 0)
                    {
                        bill.BillStatus = "Thanh toán hoàn tất";
                    }

                    var billDetail = await _billRepository.GetBillDetailHaveCusAndAppByIdAsync(payment.BillID);
                    IncomeExpenses incomeExpenses = new IncomeExpenses   // thêm vào phiếu thu
                    {
                        Amount = payment.Amount,
                        Date = payment.PaymentDate,
                        PartnerName = bill.Customer.FirstName + " " + bill.Customer.LastName,
                        BranchID = bill.Appointment.BranchID,
                        PayMethod = payment.PaymentMethod,
                        TypeOfIncome = "Thu",
                        IncomeExpensesCode = await GenerateBillCodeAsync(),                       
                    };
                    await _incomeExpensesRepository.AddncomeExpensesAsync(incomeExpenses);
                    await _billRepository.UpdateBill(bill);                  
                }

                return await _paymentRepository.AddPayment(payment);
            } catch (Exception ex) {
                return false;
            }
        }

        private bool IsValidFormat(string input)
        {
            string pattern = @"^[A-Z]{2}\d{4}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private async Task<string> GenerateBillCodeAsync()
        {
            var lastBillCode = await _incomeExpensesRepository.GetLastCodeAsync();
            if (lastBillCode == null || IsValidFormat(lastBillCode) == false)
            {
                return "PT0001";
            }
            var lastCode = lastBillCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "PT" + numericPart.ToString("D4");
        }

        public async Task<double?> GetRevenueToday()
        {
            return await _paymentRepository.GetRevenue();
        }

        public async Task<List<Payment>> GetAllPaymentsByBranch(long branchID)
        {
            var list = await _paymentRepository.GetAllPaymentByBranch(branchID);
            if (list == null)
            {
                throw new ErrorMessage("There is no data in the system");
            }
            return list;
        }

        public async Task<Object> GetPaymentByBill(long billID)
        {
            return await _paymentRepository.GetPaymentByBill(billID);
        }

        public async Task<object> Getfinance()
        {
         return await _paymentRepository.Getfinance();
        }
    }
}
