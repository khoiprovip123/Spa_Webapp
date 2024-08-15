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
    internal class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(e => e.SaleID);
            builder.Property(e => e.SaleID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.Customer)
                .WithMany(e => e.Sales)
                .HasForeignKey(e => e.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Employee)
             .WithMany(e => e.Sales)
             .HasForeignKey(e => e.EmployeeID)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
