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
    internal class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(sc => new { sc.BranchID, sc.ProductID });

            builder.HasOne(e => e.Branch)
            .WithMany(e => e.Warehouse)
            .HasForeignKey(e => e.BranchID)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Product)
             .WithMany(e => e.Warehouses)
             .HasForeignKey(e => e.ProductID)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
