using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IPaymentService
    {
        Task<bool> AddPayment(Payment payment);

        Task<double?> GetRevenueToday();

        Task<List<Payment>> GetAllPaymentsByBranch(long branchID);

        Task<Object> GetPaymentByBill(long billID);

        Task<object> Getfinance();
    }
}
