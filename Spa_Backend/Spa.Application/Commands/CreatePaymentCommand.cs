using MediatR;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreatePaymentCommand : IRequest<long>
    {
        public long PaymentID { get; set; }
        public long? CustomerID { get; set; }
        public long AppointmentID { get; set; }
        public double? Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreatePaymentCommandHander : IRequestHandler<CreatePaymentCommand, long>
    {
        private readonly IPaymentService _paymentService;

        public CreatePaymentCommandHander(IPaymentService paymentService)
        {
            _paymentService = paymentService;

        }

        public async Task<long> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            Payment payment = new Payment
            {
                //   AppointmentID = request.AppointmentID,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                //CustomerID = request.CustomerID,
                Notes = request.Notes,
                CreatedAt = request.CreatedAt,
                Status = request.Status,
                PaymentDate = DateTime.Now,
            };

            await _paymentService.AddPayment(payment);
            return payment.PaymentID;
        }
    }
}
