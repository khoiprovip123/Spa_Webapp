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
    internal class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(sc => new {sc.SaleID, sc.ProductID});

            builder.HasOne(e => e.Sale)
            .WithMany(e => e.Purchases)
            .HasForeignKey(e => e.SaleID)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Product)
             .WithMany(e => e.Purchases)
             .HasForeignKey(e => e.ProductID)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
    }

