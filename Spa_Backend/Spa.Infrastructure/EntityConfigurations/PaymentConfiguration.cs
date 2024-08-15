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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(e => e.PaymentID);
            builder.Property(e => e.PaymentID).ValueGeneratedOnAdd();

            builder.HasOne(b => b.Bill)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.BillID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
