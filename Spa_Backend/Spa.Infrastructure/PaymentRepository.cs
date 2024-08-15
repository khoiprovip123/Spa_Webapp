using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Spa.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly SpaDbContext _spaDbContext;

        public PaymentRepository(SpaDbContext spaDbContext)
        {
            _spaDbContext = spaDbContext;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            await _spaDbContext.Payments.AddAsync(payment);
            _spaDbContext.SaveChanges();
            return true;
        }


        public async Task<double?> GetRevenue()
        {
            DateTime date = DateTime.Now;
            var totalAmount = await _spaDbContext.Payments
           .Where(p => p.PaymentDate.Date == date.Date)
           .SumAsync(p => p.Amount);

            return totalAmount;
        }

        public async Task<List<Payment>> GetAllPaymentByBranch(long branchID)
        {
            return await _spaDbContext.Payments.Include(c => c.PaymentID)   ///.customer cái cũ
                .Include(a => a.PaymentID).ToListAsync();
        }

        public async Task<Object> GetPaymentByBill(long billID)
        {
            IQueryable<Payment> query = _spaDbContext.Payments.Where(a => a.BillID == billID);

            var payments = await query.ToListAsync();
            var response = payments.Select(a => new
            {
                paymentID = a.PaymentID,
                date = a.PaymentDate,
                amount = a.Amount,
                paymentMethod = a.PaymentMethod
            });

            return response;
        }

        public async Task<object> Getfinance() {
            var cash = _spaDbContext.Payments.Where(a => a.PaymentMethod == "Tiền mặt").Select(a => a.Amount).Sum();
            var bank = _spaDbContext.Payments.Where(a => a.PaymentMethod == "Chuyển khoản").Select(a => a.Amount).Sum();
            return new
            {
                cash = cash,
                bank = bank 
            };
        }



    }
}
