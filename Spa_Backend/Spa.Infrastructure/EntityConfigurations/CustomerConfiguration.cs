using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure.EntityConfigurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.CustomerID);
            builder.Property(e => e.CustomerID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.CustomerType)
                .WithMany(e => e.Customers)
                .HasForeignKey(e => e.CustomerTypeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
