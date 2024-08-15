using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Text.Json.Serialization;
using System.Text.Json;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Spa.Domain.IRepository;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IIncomeExpensesService _incomeExpensesService;
        private readonly IBillService _billService;
        private readonly IPaymentService _paymentService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        public ReportController(IBillService billService, IPaymentService paymentService, IIncomeExpensesService incomeExpensesService)
        {
            _incomeExpensesService = incomeExpensesService;
            _billService = billService;
            _paymentService = paymentService;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        }
        [HttpGet("getdetail")]
        [HasPermission(SetPermission.GetRevenueReportByBranch)]

        public async Task<ActionResult> GetRevenueReportByBranch(long idBrand, DateTime fromDate, DateTime toDate)
        {
            var billLineByDate = await _billService.GetRevenueReport(idBrand, fromDate, toDate);
            return new JsonResult(billLineByDate, _jsonSerializerOptions);
        }
        [HttpGet("getbyday")]
        [HasPermission(SetPermission.GetRevenueReportByDay)]
        public async Task<ActionResult> GetRevenueReportByDay(long idBrand, DateTime fromDate, DateTime toDate)
        {
            var billLineByDate = await _billService.GetRevenueReportByDay(idBrand, fromDate, toDate);
            return new JsonResult(billLineByDate, _jsonSerializerOptions);
        }

        [HttpGet("finance")]
        public async Task<ActionResult> GetFinance()
        {
            var finance = await _paymentService.Getfinance();
            return new JsonResult(finance, _jsonSerializerOptions);
        }

        [HttpGet("PhieuThuChi")]
        public async Task<ActionResult> AllThuChi()
        {
            var finance = await _incomeExpensesService.GetIncomes();
            return new JsonResult(finance, _jsonSerializerOptions);
        }
    }
}
