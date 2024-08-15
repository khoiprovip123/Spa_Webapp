using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Spa.Application.Commands;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Data;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator;
        private readonly IAppointmentService _appointmentService;
        private readonly IBillService _billService;

        public PaymentController(IPaymentService paymentService, IMediator mediator, IAppointmentService appointmentService, IBillService billService)
        {
            _paymentService = paymentService;
            _mediator = mediator;
            _appointmentService = appointmentService;
            _billService = billService;
        }

        [HttpGet("/GetRevenueToday")]
        [HasPermission(SetPermission.GetPaymentByDay)]
        public async Task<ActionResult> GetPaymentByDay()
        {
            var revenue = await _paymentService.GetRevenueToday();
            return Ok(new { revenue = revenue });
        }

        [HttpGet("/getpaymentofcus")]
        [HasPermission(SetPermission.GetPaymenOfCustomer)]
        public async Task<ActionResult> GetPaymenOfCustomer()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return Ok();
        }

        [HttpPost]
        [HasPermission(SetPermission.AddPayment)]
        public async Task<IActionResult> AddPayment(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
               if(await _billService.GetBillByIdAsync(payment.BillID)!= null)
                {
                    await _paymentService.AddPayment(payment);
                    return Ok(new JsonResult("Success"));
                }
                return NotFound();
            }
            catch (ErrorMessage ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("/GetPaymentByBranch")]
        [HasPermission(SetPermission.GetPaymentByBranch)]
        public async Task<ActionResult> GetPaymentByBranch(long branchID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = await _paymentService.GetAllPaymentsByBranch(branchID);
                return Ok(list);
            }
            catch (ErrorMessage ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ExportExel")]
        [HasPermission(SetPermission.ExportExelPayment)]
        public async Task<FileResult> ExportExelPayment(long branchID)
        {
            var list = await _paymentService.GetAllPaymentsByBranch(branchID);
            var filename = $"Payment_At_Branch_{branchID}.xlsx";
            return GenerateExel(filename, list);
        }

        private FileResult GenerateExel(string filename, List<Payment> payments)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable("Payment");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn ("Customer Code"),
                new DataColumn ("Customer Name"),
                new DataColumn ("Payment Date"),
                new DataColumn ("Total"),
                new DataColumn ("Payment Method"),
                new DataColumn ("Create At"),
                new DataColumn ("Note")
            });

            foreach (var payment in payments)
            {
                //    dataTable.Rows.Add(payment.Customer.CustomerCode, payment.Customer.FirstName + " " + payment.Customer.LastName, payment.PaymentDate, payment.Amount, payment.PaymentMethod, payment.CreatedAt, payment.Notes == null ? "" : payment.Notes);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                      filename);
                }
            }
        }
        [HttpGet("GetPaymentsByBill")]
        public async Task<ActionResult> GetPaymentsByBill(long idBill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = await _paymentService.GetPaymentByBill(idBill);
                return Ok(list);
            }
            catch (ErrorMessage ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
