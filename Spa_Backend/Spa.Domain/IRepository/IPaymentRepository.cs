using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IPaymentRepository
    {
        Task<bool> AddPayment(Payment payment);

        Task<double?> GetRevenue();

        Task<List<Payment>> GetAllPaymentByBranch(long branchID);

        Task<Object> GetPaymentByBill(long billID);

        Task<object> Getfinance();
    }
}
