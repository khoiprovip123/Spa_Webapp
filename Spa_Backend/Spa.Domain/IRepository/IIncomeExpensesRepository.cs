using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IIncomeExpensesRepository
    {
        Task<bool> AddncomeExpensesAsync(IncomeExpenses incomeExpenses);

        Task<string> GetLastCodeAsync();

        Task<object> GetIncomes();
    }
}
