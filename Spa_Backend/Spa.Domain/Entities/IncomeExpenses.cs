using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class IncomeExpenses
    {
        public long IncomeExpensID { get; set; }
        public string? IncomeExpensesCode { get; set; }
        public DateTime Date { get; set; }
        public string PartnerName { get; set; }
        public string PayMethod { get; set; }
        public string TypeOfIncome { get; set; }   // thu hoặc chi
        public double? Amount { get; set; }
        public long? BranchID { get; set; }
        public long? PaymentID { get; set; }
        public Payment? Payment { get; set; }
    }
}
