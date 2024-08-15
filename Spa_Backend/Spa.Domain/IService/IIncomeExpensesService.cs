using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IIncomeExpensesService
    {
        Task<bool> AddncomeExpensesAsync(IncomeExpenses incomeExpenses);

        Task<object> GetIncomes();
    }
}
