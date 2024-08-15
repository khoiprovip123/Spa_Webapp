using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure.EntityConfigurations
{
    internal class IncomeExpensesConfiguration : IEntityTypeConfiguration<IncomeExpenses>
    {
        public void Configure(EntityTypeBuilder<IncomeExpenses> builder)
        {
           builder.HasKey(c => c.IncomeExpensID);
            builder.Property(e => e.IncomeExpensID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.Payment)
                   .WithOne(e=> e.IncomeExpenses)
                   .HasForeignKey<IncomeExpenses>(a => a.PaymentID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
