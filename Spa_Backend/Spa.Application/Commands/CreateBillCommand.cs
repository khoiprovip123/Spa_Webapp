using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreateBillCommand : IRequest<long>
    {
        public long? CustomerID { get; set; }
        public long AppointmentID { get; set; }
        public DateTime? Date { get; set; }
        public string? BillStatus { get; set; }
        public string? Doctor { get; set; }
        public string? TechnicalStaff { get; set; }

        public double? TotalAmount { get; set; }   // tổng tiền
        public double? AmountInvoiced { get; set; } = 0;// thanh toán
        public double? AmountResidual { get; set; } = 0; // còn lại
        public string? KindofDiscount { get; set; }
        public string? Note { get; set; }
        public double? AmountDiscount { get; set; }
        public ICollection<BillItem>? BillItems { get; set; }
    }

    public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, long>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IBillService _billService;

        public CreateBillCommandHandler(IBillService billService, IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
            _billService = billService;
        }

        private bool IsValidFormat(string input)
        {
            string pattern = @"^[A-Z]{2}\d{4}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private async Task<string> GenerateBillCodeAsync()
        {
            var lastBillCode = await _billService.GetLastCodeAsync();
            if (lastBillCode == null || IsValidFormat(lastBillCode) == false)
            {
                return "HD0001";
            }
            var lastCode = lastBillCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "HD" + numericPart.ToString("D4");
        }
        public async Task<long> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            long idNewBill = 0;
            Bill bill = new Bill()
            {
                BillCode = await GenerateBillCodeAsync(),
                AppointmentID = request.AppointmentID,
                AmountResidual = request.TotalAmount,
                AmountInvoiced = request.AmountInvoiced,
                TechnicalStaff = request.TechnicalStaff,
                Doctor = request.Doctor,
                TotalAmount = request.TotalAmount,
                KindofDiscount = request.KindofDiscount,
                Note = request.Note,
                AmountDiscount = request.AmountDiscount,
                BillStatus = request.BillStatus,
                CustomerID = request.CustomerID,
                Date = DateTime.Now,
                BillItems = request.BillItems,
            
            };
            var newBill = await _billService.CreateBill(bill);
            return idNewBill;
        }
    }
}
